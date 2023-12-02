using ChineseObjects.Lang.Declaration;
using LLVMSharp.Interop;

namespace ChineseObjects.Lang.CodeGen;

public class LLVMCodeGen : ITypesAwareStatementVisitor<LLVMValueRef>
{
    private LLVMContextRef ctx;
    private LLVMModuleRef module;
    private LLVMBuilderRef builder;

    /**
     * Contains function types for all ever _declared_ functions.
     * Due to a limitation of LLVMSharp (limited access to the original LLVM API), we are forced to store them in a
     * dictionary.
     */
    private Dictionary<string, LLVMTypeRef> funcType = new();
    /**
     * Contains struct types declared for each of the classes, as well as structs corresponding to primitive types.
     * Due to a limitation of LLVMSharp (limited access to the original LLVM API), we are forced to store them in a
     * dictionary.
     */
    private Dictionary<string, LLVMTypeRef> Struct = new();

    /**
     * Generic pointer type.
     *
     * Represented as a pointer to the `Int1` type, as LLVMSharp does not give a way to represent an opaque pointer.
     */
    private readonly LLVMTypeRef OpaquePtr; // Note: can't be declared static as must be a type from the `ctx` context.

    /**
     * Convert a type to a pointer of that type.
     *
     * Note: unless opaque pointers are disabled (which is not possible with the current upstream version of LLVMSharp),
     * pointers to all types appear as a universal "ptr" type.
     */
    private LLVMTypeRef AsPtr(LLVMTypeRef typ)
    {
        return LLVMTypeRef.CreatePointer(typ, 0);
    }
    
    /**
     * Get a type that is a pointer to the struct with the given name.
     */
    private LLVMTypeRef StructPtr(string name)
    {
        return AsPtr(Struct[name]);
    }
    
    
    public LLVMCodeGen()
    {
        ctx = LLVMContextRef.Create();
        module = ctx.CreateModuleWithName("main_module");
        builder = ctx.CreateBuilder();
        OpaquePtr = LLVMTypeRef.CreatePointer(ctx.Int1Type, 0);

        // TODO: move native methods implementation to a separate class

        funcType.Add("malloc",
            LLVMTypeRef.CreateFunction(LLVMTypeRef.CreatePointer(ctx.Int8Type, 0), new[] { ctx.Int32Type }));
        var malloc = module.AddFunction("malloc", funcType["malloc"]);
        malloc.Linkage = LLVMLinkage.LLVMExternalLinkage;

        funcType.Add("exit", LLVMTypeRef.CreateFunction(ctx.VoidType, new[] { ctx.Int32Type }));
        var exit = module.AddFunction("exit", funcType["exit"]);
        exit.Linkage = LLVMLinkage.LLVMExternalLinkage;
        
        // Build the primitive Bool type (available globally)
        var Bool = Struct["Bool"] = ctx.CreateNamedStruct("Bool");
        Bool.StructSetBody(new []{LLVMTypeRef.Int1}, false);
        
        LLVMTypeRef PBool = LLVMTypeRef.CreatePointer(Bool, 0);
        
        // Compile `Bool.And`

        string funcName = "Bool.And..Bool";
        LLVMValueRef func = module.GetNamedFunction(funcName);
        if (func.BasicBlocks.Length != 0)
        {
            throw new LLVMGenException("Function " + funcName + " already has a body");
        }

        var parames = new LLVMTypeRef[]
        {
            /*this*/PBool,
            /*other*/PBool
        };

        funcType[funcName] = LLVMTypeRef.CreateFunction(PBool, parames);
        func = module.AddFunction(funcName, funcType[funcName]);
        func.Linkage = LLVMLinkage.LLVMExternalLinkage;
        var pThis = func.GetParam(0);
        var pOther = func.GetParam(1);
        pThis.Name = "this";
        pOther.Name = "other";
        
        builder.PositionAtEnd(func.AppendBasicBlock("entry"));
        var u1 = builder.BuildStructGEP2(Bool, pThis, 0, "unbox1");
        var u2 = builder.BuildStructGEP2(Bool, pOther, 0, "unbox2");
        var v1 = builder.BuildLoad2(ctx.Int1Type, u1, "val1");
        var v2 = builder.BuildLoad2(ctx.Int1Type, u2, "val2");
        LLVMValueRef res = builder.BuildAnd(v1, v2, "and");
        /* resptr points both to the structure and to its initial field */
        LLVMValueRef resptr = builder.BuildMalloc(Bool, "resptr");
        builder.BuildStore(res, resptr);

        builder.BuildRet(resptr);

        func.VerifyFunction(LLVMVerifierFailureAction.LLVMPrintMessageAction);

        // Compile `Bool.TerminateExecution`

        funcName = "Bool.TerminateExecution..";
        func = module.GetNamedFunction(funcName);
        if (func.BasicBlocks.Length != 0)
        {
            throw new LLVMGenException("Function " + funcName + " already has a body");
        }

        parames = new LLVMTypeRef[]
        {
            /*this*/PBool
        };

        funcType[funcName] = LLVMTypeRef.CreateFunction(PBool, parames);
        func = module.AddFunction(funcName, funcType[funcName]);
        func.Linkage = LLVMLinkage.LLVMExternalLinkage;
        pThis = func.GetParam(0);
        pThis.Name = "this";

        builder.PositionAtEnd(func.AppendBasicBlock("entry"));
        var unboxed = builder.BuildStructGEP2(Bool, pThis, 0, "unboxed");
        builder.BuildCall2(funcType["exit"], exit,
            new[] { builder.BuildIntCast(builder.BuildLoad2(ctx.Int1Type, unboxed), ctx.Int32Type) });
        builder.BuildRet(LLVMValueRef.CreateConstPointerNull(PBool));
        func.VerifyFunction(LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }

    public void CheckAndDump()
    {
        module.Dump();
        module.Verify(LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }

    public void Compile(ITypesAwareProgram program)
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
        funcType[entrypointName] = LLVMTypeRef.CreateFunction(ctx.VoidType, new LLVMTypeRef[] { });
        LLVMValueRef co_entry = module.AddFunction(entrypointName, funcType[entrypointName]);
        co_entry.Linkage = LLVMLinkage.LLVMExternalLinkage;
        builder.PositionAtEnd(co_entry.AppendBasicBlock("entry"));
        // TODO: first construct an object of type `Main` and call the method with that value rather than a null pointer
        builder.BuildCall2(funcType[mainName], main, new[] { LLVMValueRef.CreateConstNull(OpaquePtr),  });
        builder.BuildRetVoid();
    }

    private void DeclareClass(ITypesAwareClassDeclaration cls)
    {
        var Class = Struct[cls.ClassName()] = ctx.CreateNamedStruct(cls.ClassName());
        Class.StructSetBody(Enumerable.Repeat(OpaquePtr, cls.VariableDeclarations().Count()).ToArray(), Packed: false);
        
        foreach (ITypesAwareMethod method in cls.MethodDeclarations())
        {
            string funcName = cls.ClassName() + '.' + method.MethodName() + ".." +
                              String.Join('.', method.Parameters().GetParameters().Select(x => x.Type().TypeName().Value()));
            string retName = method.ReturnType().TypeName().Value();
            var retT = StructPtr(retName);  // Pointer to struct will be returned
            funcType[funcName] = LLVMTypeRef.CreateFunction(retT,
                Enumerable.Repeat(OpaquePtr, 1 + method.Parameters().GetParameters().Count()).ToArray());
            var func = module.AddFunction(funcName, funcType[funcName]);
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
        if (!funcType.ContainsKey(funcName))
        {
            throw new LLVMGenException("Function " + funcName + " is not declared");
        }

        LLVMValueRef func = module.GetNamedFunction(funcName);
        
        // Compile all arguments' values (other than "this")
        IEnumerable<LLVMValueRef> args = methodCall.Arguments().Values().Select(arg => arg.Value().AcceptVisitor(this));
        // The caller itself (aka "this") is an implicit first argument to every method call:
        args = args.Prepend(methodCall.Caller().AcceptVisitor(this));
        // Build the method call
        return builder.BuildCall2(funcType[funcName], func, args.ToArray());
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

    public LLVMValueRef Visit(ITypedArgument _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(ITypedClassInstantiation _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(ITypesAwareReturn _)
    {
        throw new NotImplementedException();
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
