using System.Diagnostics;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration.Parameter;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

using LLVMSharp.Interop;

namespace ChineseObjects.Lang.CodeGen;

/*
 * WARNING: avoid using potentially lazy containers (such as `IEnumerable`) when working with llvm objects that need to
 * be built. Builder heavily relies on the order of instruction builds and its consistency with other builder operations
 * (such as changing its position).
 * Instead use structures that use strict evaluation (such as `List`)
 */

/// <summary>
/// Code generator for ChineseObjects
/// </summary>
public class CompiledProgram : ITypesAwareStatementVisitor<LLVMValueRef>
{
    private readonly ITypesAwareProgram _program;
    
    private LLVMContextRef ctx;
    private LLVMModuleRef module;
    private LLVMBuilderRef builder;

    /// <summary>
    /// Contains function types for all ever _declared_ functions.
    /// Due to a limitation of LLVMSharp (limited access to the original LLVM API), we are forced to store them in a
    /// dictionary.
    /// </summary>
    private Dictionary<string, LLVMTypeRef> FuncType;

    /// <summary>
    /// Contains struct types declared for each of the classes, as well as structs corresponding to primitive types.
    /// Due to a limitation of LLVMSharp (limited access to the original LLVM API), we are forced to store them in a
    /// dictionary.
    /// </summary>
    private Dictionary<string, LLVMTypeRef> Struct;

    /// <summary>
    /// Generic pointer type.
    ///
    /// Represented as a pointer to the `Int1` type, as LLVMSharp does not give a way to represent an opaque pointer.
    /// </summary>
    private readonly LLVMTypeRef OpaquePtr; // Note: can't be declared static as must be a type from the `ctx` context.

    /// <summary>
    /// Mapping (for current method's scope) from name (of either a local variable or a class field) to a pointer value
    /// that stores a reference to the corresponding object.
    /// </summary>
    ///
    /// <example>
    /// <code>
    /// LLVMTypeRef varType = ...;  // Type of the _object_ that variable is supposed to reference
    /// LLVMTypeRef varPtr = LLVMTypeRef.CreatePointer(varType, 0);
    /// string varName = "abc";  // Original variable name
    ///
    /// LLVMValueRef refer = nameToReferencer[varName];  // Points to where object reference is
    /// LLVMValueRef obj = builder.BuildLoad(varPtr, refer, "objRef");  // Pointer to the structure
    /// int fieldN = 123;  // Index of the field to access
    /// LLVMValueRef field = builder.BuildStructGEP2(varType, obj, fieldN, "field");  // Pointer to the field
    /// </code>
    /// </example>
    private Dictionary<string, LLVMValueRef> nameToReferencer = new();

    /// <summary>
    /// Convert a type to a pointer of that type.
    ///
    /// Note: unless opaque pointers are disabled (which is not possible with the current upstream version of LLVMSharp),
    /// pointers to all types appear as a universal "ptr" type.
    /// </summary>
    /// <param name="typ">Pointee type</param>
    /// <returns>Pointer</returns>
    private LLVMTypeRef AsPtr(LLVMTypeRef typ)
    {
        return LLVMTypeRef.CreatePointer(typ, 0);
    }
    
    /// <summary>
    /// Get a type that is a pointer to the struct with the given name
    /// </summary>
    /// <param name="name">Pointee struct name</param>
    /// <returns>Pointer</returns>
    private LLVMTypeRef StructPtr(string name)
    {
        return AsPtr(Struct[name]);
    }

    /// <param name="cls">Class where the method is defined</param>
    /// <param name="method">Method to get name of</param>
    /// <returns>The name that the method will have in LLVM IR</returns>
    private static string FuncName(ITypesAwareClassDeclaration cls, ITypesAwareMethod method)
    {
        return cls.ClassName() + '.' + method.MethodName() + ".." + String.Join('.',
            method.Parameters().GetParameters().Select(p => p.Type().TypeName().Value()));
    }

    /// <param name="type">Type corresponding to the class where the method is defined</param>
    /// <param name="method">Method to get name of</param>
    /// <returns>The name that the method will have in LLVM IR</returns>
    private static string FuncName(Type type, ITypedMethodCall method)
    {
        return type.TypeName().Value() + '.' + method.MethodName() + ".." + String.Join('.',
            method.Arguments().Values().Select(p => p.Type().TypeName().Value()));
    }

    /// <summary>
    /// Represents constructor of a class as its method that has the same parameters, an empty name, and a body
    /// of the constructor followed by a "return this" statement
    /// </summary>
    /// <param name="cls">Class</param>
    /// <param name="ctor">Constructor of the class</param>
    /// <returns>Pseudo method of the class</returns>
    private static ITypesAwareMethod ConstructorAsMethod(ITypesAwareClassDeclaration cls, ITypesAwareConstructor ctor)
    {
        Type type = cls.SelfType();
        return new TypesAwareMethod("", ctor.Parameters(), type,
            new TypesAwareStatementsBlock(ctor.Body().Statements().Append(new TypesAwareReturn(new TypedThis(type)))));
    }


    /// <summary>
    /// Compile a type aware program, given an `LLVMExposingCodeGen` with compiled native types.
    /// </summary>
    /// <param name="g">Generator with native types compiled. Once passed to this constructor, it shall not be used
    /// anymore</param>
    /// <param name="program">Program to compile</param>
    public CompiledProgram(LLVMExposingCodeGen g, ITypesAwareProgram program)
    {
        _program = program;

        /*
         * Native types were generated with `g`. Now we can access the environment prepared for us
         * (and `g` shall not be used by anyone anymore).
         */
        ctx = g.ctx;
        module = g.module;
        builder = g.builder;
        FuncType = g.FuncType;
        Struct = g.Struct;
        OpaquePtr = g.OpaquePtr;

        CompileProgram(program);
    }

    public int MakeExecutable()
    {
        //module.Dump();
        module.Verify(LLVMVerifierFailureAction.LLVMPrintMessageAction);
        
        LLVM.InitializeAllTargetInfos();
        LLVM.InitializeAllTargets();
        LLVM.InitializeAllTargetMCs();
        LLVM.InitializeAllAsmParsers();
        LLVM.InitializeAllAsmPrinters();
        var trpl = LLVMTargetRef.GetTargetFromTriple(LLVMTargetRef.DefaultTriple);
        var machine = trpl.CreateTargetMachine(LLVMTargetRef.DefaultTriple, "", "",
            LLVMCodeGenOptLevel.LLVMCodeGenLevelNone, LLVMRelocMode.LLVMRelocDefault,
            LLVMCodeModel.LLVMCodeModelDefault);
        const string objName = "CO_user.o";
        const string exeName = "run.out";
        machine.EmitToFile(module, objName, LLVMCodeGenFileType.LLVMObjectFile);

        const string cmain = "extern void _CO_entrypoint();int main(){_CO_entrypoint();}";
        
        string fname = Path.GetTempFileName();
        try
        {
            File.WriteAllText(fname + ".c", cmain);
            var gcc = Process.Start("gcc", new[] { "-o", exeName, fname + ".c", objName });
            gcc.WaitForExit();
            return gcc.ExitCode;
        }
        finally
        {
            // Try to delete the temporary files. Ignore whatever errors might appear
            try { File.Delete(fname); } catch {}
            try { File.Delete(fname + ".c"); } catch {}
        }
    }

    private void CompileProgram(ITypesAwareProgram program)
    {
        foreach (ITypesAwareClassDeclaration cls in program.ClassDeclarations())
        {
            DeclareClass(cls);
        }
        foreach (ITypesAwareClassDeclaration cls in program.ClassDeclarations())
        {
            CompileClass(cls);
        }

        // Compile _CO_entrypoint. This function shall be the entry point of ChineseObjects' runtime
        string mainName = "Main...";
        LLVMValueRef main = module.GetNamedFunction(mainName);
        if (main.BasicBlocks.Length == 0)
        {
            throw new LLVMGenException("Could not find class Main with parameterless constructor");
        }

        string entrypointName = "_CO_entrypoint";
        FuncType[entrypointName] = LLVMTypeRef.CreateFunction(ctx.VoidType, new LLVMTypeRef[] { });
        LLVMValueRef co_entry = module.AddFunction(entrypointName, FuncType[entrypointName]);
        co_entry.Linkage = LLVMLinkage.LLVMExternalLinkage;
        builder.PositionAtEnd(co_entry.AppendBasicBlock("entry"));
        builder.BuildCall2(FuncType[mainName], main, new[] { builder.BuildMalloc(Struct["Main"], "mainObj") });
        builder.BuildRetVoid();
        
        module.Verify(LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }

    private void DeclareClass(ITypesAwareClassDeclaration cls)
    {
        var Class = Struct[cls.ClassName()] = ctx.CreateNamedStruct(cls.ClassName());
        Class.StructSetBody(Enumerable.Repeat(OpaquePtr, cls.VariableDeclarations().Count()).ToArray(), Packed: false);

        // Collect normal methods and pseudo-methods generated from constructors. Declare them with the same rule
        List<ITypesAwareMethod> methods = cls.MethodDeclarations()
            .Concat(cls.ConstructorDeclarations().Select(c => ConstructorAsMethod(cls, c))).ToList();
        
        foreach (ITypesAwareMethod method in methods)
        {
            /*
             * The name of the compiled method is formed as [ClassName].[MethodName]..[Param1TypeName].[Param2TypeName]...
             * For instance, a method `Hello` of class `Abc` that explicitly accepts two parameters of types `Bool` and
             * `Abc` respectively will be called "Abc.Hello..Bool.Abc". Note that all methods also implicitly accept
             * the object method is called on as their initial argument. Thus, the method that is compiled into
             * a routine called "Abc.Hello..Bool.Abc" shall be called with three arguments: of types `Abc`, `Bool`, and
             * `Abc` respectively (the object itself and two parameters).
             *
             * Constructors are treated like methods with empty names.
             */
            string funcName = FuncName(cls, method);
            var retT = OpaquePtr;  // Pointer to struct will be returned
            FuncType[funcName] = LLVMTypeRef.CreateFunction(retT,
                Enumerable.Repeat(OpaquePtr, 1 + method.Parameters().GetParameters().Count()).ToArray());
            var func = module.AddFunction(funcName, FuncType[funcName]);
            func.Linkage = LLVMLinkage.LLVMExternalLinkage;

            uint i = 0;
            var fparam = func.GetParam(0);
            fparam.Name = "this";
            foreach (ITypedParameter param in method.Parameters().GetParameters())
            {
                ++i;
                fparam = func.GetParam(i);
                fparam.Name = param.Name();
            }
        }
    }

    private void CompileClass(ITypesAwareClassDeclaration cls)
    {
        // TODO 3: support inheritance
        
        /*
         * Now a constructor is essentially a method that is run on a newly allocated memory and returns the value of
         * "this". So let's treat them as methods!
         */
        List<ITypesAwareMethod> methods = cls.MethodDeclarations()
            .Concat(cls.ConstructorDeclarations()
                .Select(ctor => ConstructorAsMethod(cls, ctor))).ToList();

        foreach (ITypesAwareMethod method in methods)
        {
            string funcName = FuncName(cls, method);
            LLVMValueRef func = module.GetNamedFunction(funcName);
            builder.PositionAtEnd(func.AppendBasicBlock("entry"));
            
            // `nameToReferencer` keeps fields and local variables in the current scope. Initially set it up with
            // class fields and method parameters.
            //
            // **The order is important**: method parameters shadow fields, not vice versa.
            nameToReferencer.Clear();

            LLVMValueRef self = func.GetParam(0);
            nameToReferencer["this"] = self;  // "this" is a special case: unlike all others, which are pointers to
                                             // pointers, "this" is a direct pointer (as it can't be assigned and is
                                             // anyway handled separately everywhere.
            uint fieldN = 0;
            foreach (ITypedVariable field in cls.VariableDeclarations())
            {
                // As all fields of structs are pointers to other objects, the result of `GEP` is already a pointer
                // to pointer, don't need to do anything additionally.
                nameToReferencer[field.Name()] =
                    builder.BuildStructGEP2(Struct[cls.ClassName()], self, fieldN, field.Name());
                ++fieldN;
            }

            uint paramN = 1;  // Start with 1 because of implicit "this"
            foreach (ITypedParameter param in method.Parameters().GetParameters())
            {
                // The value returned by `func.GetParam()` is a direct pointer but a name should be dynamically bound
                // to a reference, so an additional layer of indirection is needed. The "l_*" nodes are this additional
                // layer of indirection.
                LLVMValueRef referer = builder.BuildAlloca(OpaquePtr, "l_" + param.Name());
                builder.BuildStore(func.GetParam(paramN), referer);
                nameToReferencer[param.Name()] = referer;
                ++paramN;
            }
            
            // Now compile statements
            BuildStatements(method.Body().Statements());

            // Every basic block must end with a terminator instruction. If the function does not return, return a
            // nil ref
            if (builder.InsertBlock.LastInstruction.IsATerminatorInst.Handle == IntPtr.Zero)
            {
                builder.BuildRet(LLVMValueRef.CreateConstNull(OpaquePtr));
            }
        }
    }

    private void BuildStatements(IEnumerable<ITypesAwareStatement> stmts)
    {
        foreach (ITypesAwareStatement stmt in stmts)
        {
            // Build the statement. It is built and inserted to the builder position, which should remain
            // at the end of the function after every statement is built.
            stmt.AcceptVisitor(this);
        }
    }

    public LLVMValueRef Visit(ITypedBoolLiteral boolLit)
    {
        var Bool = Struct["Bool"];
        var boxed = builder.BuildMalloc(Bool, "boxed");
        var direct = builder.BuildStructGEP2(Bool, boxed, 0, "direct");
        builder.BuildStore(LLVMValueRef.CreateConstInt(ctx.Int1Type, boolLit.Value() ? 1UL : 0), direct);
        return boxed;
    }

    public LLVMValueRef Visit(ITypedNumLiteral numLit)
    {
        var Number = Struct["Number"];
        var boxed = builder.BuildMalloc(Number, "boxed");
        var direct = builder.BuildStructGEP2(Number, boxed, 0, "direct");
        builder.BuildStore(LLVMValueRef.CreateConstInt(ctx.Int32Type, (ulong)numLit.Value()), direct);
        
        return boxed;
        
    }

    public LLVMValueRef Visit(ITypedMethodCall methodCall)
    {
        string funcName = FuncName(methodCall.Caller().Type(), methodCall);
        if (!FuncType.ContainsKey(funcName))
        {
            throw new LLVMGenException("Function " + funcName + " is not declared");
        }

        LLVMValueRef func = module.GetNamedFunction(funcName);
        
        // Compile all arguments' values (other than "this")
        List<LLVMValueRef> args = methodCall.Arguments().Values().Select(arg => arg.Value().AcceptVisitor(this))
            .ToList();
        // The caller itself (aka "this") is an implicit first argument to every method call:
        args = args.Prepend(methodCall.Caller().AcceptVisitor(this)).ToList();
        // Build the method call
        return builder.BuildCall2(FuncType[funcName], func, args.ToArray());
    }

    public LLVMValueRef Visit(ITypedReference tRef)
    {
        return builder.BuildLoad2(OpaquePtr, nameToReferencer[tRef.Name()]);
    }

    public LLVMValueRef Visit(ITypedThis tThis)
    {
        // Unlike all other names, which are indirect pointers, "this" is a direct pointer
        return nameToReferencer["this"];
    }

    public LLVMValueRef Visit(ITypedArgument arg)
    {
        return arg.Value().AcceptVisitor(this);
    }

    public LLVMValueRef Visit(ITypedClassInstantiation classInstantiation)
    {
        ITypesAwareClassDeclaration cls = _program
            .ClassDeclarations()
            .First(classDeclaration => classDeclaration.ClassName().Equals(classInstantiation.ClassName()));
        ITypesAwareConstructor firstMatchedConstructor = cls
            .ConstructorDeclarations()
            .First(decl => Type.ConstructorSignatureCheck(decl, classInstantiation.Arguments()));
        
        ITypesAwareMethod asMethod = ConstructorAsMethod(cls, firstMatchedConstructor);
        string funcName = FuncName(cls, asMethod);
        if (!FuncType.ContainsKey(funcName))
        {
            throw new LLVMGenException("Function " + funcName + " is note declared");
        }

        LLVMValueRef func = module.GetNamedFunction(funcName);

        LLVMValueRef alloc = builder.BuildMalloc(Struct[cls.ClassName()]);
        
        List<LLVMValueRef> args = classInstantiation.Arguments().Values().Select(arg => arg.Value().AcceptVisitor(this))
            .ToList();
        // Add implicit initial argument "this"
        args = args.Prepend(alloc).ToList();

        return builder.BuildCall2(FuncType[funcName], func, args.ToArray(), "new");
    }

    public LLVMValueRef Visit(ITypesAwareReturn val)
    {
        return builder.BuildRet(val.Expression().AcceptVisitor(this));
    }

    public LLVMValueRef Visit(ITypesAwareAssignment asgn)
    {
        return builder.BuildStore(asgn.Expr().AcceptVisitor(this), nameToReferencer[asgn.Name()]);
    }

    public LLVMValueRef UnboxCond(ITypedExpression expr)
    {
        LLVMValueRef boxedCond = expr.AcceptVisitor(this);
        LLVMValueRef cond = builder.BuildLoad2(ctx.Int1Type, boxedCond, "cond");
        return cond;
    }

    public LLVMValueRef Visit(ITypesAwareWhile tWhile)
    {
        LLVMBasicBlockRef condCheck = builder.InsertBlock.Parent.AppendBasicBlock("loop_cond");
        LLVMBasicBlockRef lbody = builder.InsertBlock.Parent.AppendBasicBlock("loop");
        LLVMBasicBlockRef lout = builder.InsertBlock.Parent.AppendBasicBlock("loop_out");

        builder.BuildBr(condCheck);
        builder.PositionAtEnd(condCheck);
        LLVMValueRef cond = UnboxCond(tWhile.Condition());
        builder.BuildCondBr(cond, lbody, lout);

        builder.PositionAtEnd(lbody);
        BuildStatements(tWhile.Body().Statements());
        builder.BuildBr(condCheck);  /* Jumping not to the current insert block but to where loop starts */
        lbody = builder.InsertBlock;  /* Could have been moved to another block by nested control flow! */

        builder.PositionAtEnd(lout);
        return builder.InsertBlock.LastInstruction;  /* Nothing useful we can return */
    }

    public LLVMValueRef Visit(ITypesAwareIfElse ife)
    {
        LLVMBasicBlockRef thenB, elseB, mergeB;
        thenB = builder.InsertBlock.Parent.AppendBasicBlock("then");
        mergeB = builder.InsertBlock.Parent.AppendBasicBlock("merge");
        elseB = ife.Else() is null ? mergeB : builder.InsertBlock.Parent.AppendBasicBlock("else");

        LLVMValueRef cond = UnboxCond(ife.Condition());

        builder.BuildCondBr(cond, thenB, elseB);

        builder.PositionAtEnd(thenB);
        BuildStatements(ife.Then().Statements());
        builder.BuildBr(mergeB);
        thenB = builder.InsertBlock;  /* Could have been moved to another block by nested control flow! */
        if (ife.Else() is var e && e is not null)
        {
            builder.PositionAtEnd(elseB);
            BuildStatements(e.Statements());
            builder.BuildBr(mergeB);
            elseB = builder.InsertBlock;  /* Could have been moved to another block by nested control flow! */
        }
        builder.PositionAtEnd(mergeB);
        return builder.InsertBlock.LastInstruction;  /* Nothing useful we can return */
    }
}
