using System.Diagnostics;
using ChineseObjects.Lang.Declaration;
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
        string mainName = "Main.Main..";
        LLVMValueRef main = module.GetNamedFunction(mainName);
        if (main.BasicBlocks.Length == 0)
        {
            throw new LLVMGenException("Could not find class Main with method Main()");
        }

        string entrypointName = "_CO_entrypoint";
        FuncType[entrypointName] = LLVMTypeRef.CreateFunction(ctx.VoidType, new LLVMTypeRef[] { });
        LLVMValueRef co_entry = module.AddFunction(entrypointName, FuncType[entrypointName]);
        co_entry.Linkage = LLVMLinkage.LLVMExternalLinkage;
        builder.PositionAtEnd(co_entry.AppendBasicBlock("entry"));
        // TODO: first construct an object of type `Main` and call the method with that value rather than a null pointer
        builder.BuildCall2(FuncType[mainName], main, new[] { LLVMValueRef.CreateConstNull(OpaquePtr),  });
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

        List<ITypesAwareConstructor> ctors;
        if (cls.ConstructorDeclarations().Any())
        {
            ctors = cls.ConstructorDeclarations().ToList();
        }
        else
        {
            // If there are no constructors, add a trivial empty constructor
            ctors = new List<ITypesAwareConstructor>
            {
                new TypesAwareConstructor(new TypesAwareParameters(new ITypedParameter[] { }),
                    new TypesAwareStatementsBlock(new ITypesAwareStatement[] { }))
            };
        }
        
        /*
         * Now a constructor is essentially a method that is run on a newly allocated memory and returns the value of
         * "this". So let's treat them as methods!
         */
        List<ITypesAwareMethod> methods = cls.MethodDeclarations()
            .Concat(ctors.Select(ctor => ConstructorAsMethod(cls, ctor))).ToList();


        foreach (ITypesAwareMethod method in methods)
        {
            string funcName = FuncName(cls, method);
            LLVMValueRef func = module.GetNamedFunction(funcName);
            builder.PositionAtEnd(func.AppendBasicBlock("entry"));
            foreach (ITypesAwareStatement statement in method.Body().Statements())
            {
                // Build the statement. It is built and inserted to the builder position, which should remain
                // at the end of the function after every statement is built.
                statement.AcceptVisitor(this);
            }

            // Every basic block must end with a terminator instruction. If the function does not return, return a
            // nil ref
            if (builder.InsertBlock.LastInstruction.IsATerminatorInst.Handle == IntPtr.Zero)
            {
                builder.BuildRet(LLVMValueRef.CreateConstNull(OpaquePtr));
            }
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
        throw new NotImplementedException();
        // TODO: should be boxed (allocated on heap)
        return LLVMValueRef.CreateConstReal(ctx.DoubleType, numLit.Value());
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
        // TODO: implement! The implementation is probably similar to that of `ITypedThis` (?)
        // Note the returned node is a pointer (via `StructPtr`), not a struct (from `Struct`). That's because values
        // passed around in the code are **always** boxed, so they come in forms of pointers to structs allocated on
        // heap.
        return LLVMValueRef.CreateConstNull(StructPtr(tRef.Type().TypeName().Value()));
    }

    public LLVMValueRef Visit(ITypedThis tThis)
    {
        // TODO: implement! The implementation is probably similar to that of `TypedReference` (?)
        // Note the returned node is a pointer (via `StructPtr`), not a struct (from `Struct`). That's because values
        // passed around in the code are **always** boxed, so they come in forms of pointers to structs allocated on
        // heap.
        return LLVMValueRef.CreateConstNull(StructPtr(tThis.Type().TypeName().Value()));
    }

    public LLVMValueRef Visit(ITypedParameter _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(ITypedVariable _)
    {
        throw new NotImplementedException();
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

    public LLVMValueRef Visit(ITypesAwareWhile _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(ITypesAwareIfElse _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(ITypesAwareAssignment _)
    {
        throw new NotImplementedException();
    }
}
