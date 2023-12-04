using ChineseObjects.Lang.Declaration;
using LLVMSharp.Interop;

namespace ChineseObjects.Lang.CodeGen;

/// <summary>
/// Code generator for ChineseObjects
/// </summary>
public class CompiledProgram : ITypesAwareStatementVisitor<LLVMValueRef>
{
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


    /// <summary>
    /// Compile a type aware program, given an `LLVMExposingCodeGen` with compiled native types.
    /// </summary>
    /// <param name="g">Generator with native types compiled. Once passed to this constructor, it shall not be used
    /// anymore</param>
    /// <param name="program">Program to compile</param>
    public CompiledProgram(LLVMExposingCodeGen g, ITypesAwareProgram program)
    {
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
        
        module.Dump();
        module.Verify(LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }

    private void DeclareClass(ITypesAwareClassDeclaration cls)
    {
        var Class = Struct[cls.ClassName()] = ctx.CreateNamedStruct(cls.ClassName());
        Class.StructSetBody(Enumerable.Repeat(OpaquePtr, cls.VariableDeclarations().Count()).ToArray(), Packed: false);
        
        foreach (ITypesAwareMethod method in cls.MethodDeclarations())
        {
            /*
             * The name of the compiled method is formed as [ClassName].[MethodName]..[Param1TypeName].[Param2TypeName]...
             * For instance, a method `Hello` of class `Abc` that explicitly accepts two parameters of types `Bool` and
             * `Abc` respectively will be called "Abc.Hello..Bool.Abc". Note that all methods also implicitly accept
             * the object method is called on as their initial argument. Thus, the method that is compiled into
             * a routine called "Abc.Hello..Bool.Abc" shall be called with three arguments: of types `Abc`, `Bool`, and
             * `Abc` respectively (the object itself and two parameters).
             */
            string funcName = cls.ClassName() + '.' + method.MethodName() + ".." +
                              String.Join('.', method.Parameters().GetParameters().Select(x => x.Type().TypeName().Value()));
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
        // TODO 2: compile constructors
        // TODO 3: support inheritance

        foreach (ITypesAwareMethod method in cls.MethodDeclarations())
        {
            string funcName = cls.ClassName() + '.' + method.MethodName() + ".." +
                              String.Join('.', method.Parameters().GetParameters().Select(x => x.Type().TypeName().Value()));
            LLVMValueRef func = module.GetNamedFunction(funcName);
            builder.PositionAtEnd(func.AppendBasicBlock("entry"));
            foreach (ITypesAwareStatement statement in method.Body().Statements())
            {
                // Build the statement. It is built and inserted to the builder position, which should remain
                // at the end of the function after every statement is built.
                statement.AcceptVisitor(this);
            }

            // Return a nil reference in case the execution did not reach a return statement.
            builder.BuildRet(LLVMValueRef.CreateConstNull(OpaquePtr));
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
        string funcName = methodCall.Caller().Type().TypeName().Value() + "." + methodCall.MethodName() + ".." +
                          String.Join('.',
                              methodCall.Arguments().Values().Select(arg => arg.Value().Type().TypeName().Value()));
        if (!FuncType.ContainsKey(funcName))
        {
            throw new LLVMGenException("Function " + funcName + " is not declared");
        }

        LLVMValueRef func = module.GetNamedFunction(funcName);
        
        // Compile all arguments' values (other than "this")
        IEnumerable<LLVMValueRef> args = methodCall.Arguments().Values().Select(arg => arg.Value().AcceptVisitor(this));
        // The caller itself (aka "this") is an implicit first argument to every method call:
        args = args.Prepend(methodCall.Caller().AcceptVisitor(this));
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

    public LLVMValueRef Visit(ITypedClassInstantiation _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(ITypesAwareReturn val)
    {
        return builder.BuildRet(val.AcceptVisitor(this));
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
