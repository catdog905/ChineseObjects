using ChineseObjects.Lang.Declaration;
using LLVMSharp;

namespace ChineseObjects.Lang.CodeGen;

public class LLVMCodeGen : ITypesAwareStatementVisitor<LLVMValueRef>
{
    private readonly LLVMModuleRef _module = LLVM.ModuleCreateWithName("MainModule");
    private readonly LLVMBuilderRef _builder = LLVM.CreateBuilder();

    public LLVMCodeGen()
    {
        // TODO: move native methods implementation to a separate class
        
        LLVMValueRef malloc = LLVM.AddFunction(_module, "malloc",
            LLVM.FunctionType(LLVM.PointerType(LLVM.Int8Type(), 0), new[] { LLVM.Int32Type() }, false));
        LLVM.SetLinkage(malloc, LLVMLinkage.LLVMExternalLinkage);
        LLVMValueRef exit = LLVM.AddFunction(_module, "exit",
            LLVM.FunctionType(LLVM.VoidType(), new[] { LLVM.Int32Type() }, false));
        LLVM.SetLinkage(exit, LLVMLinkage.LLVMExternalLinkage);
        
        // Build the primitive Bool type
        LLVMTypeRef TheBool = LLVM.StructType(new[] { LLVM.Int1Type() }, false);
        LLVMTypeRef PBool = LLVM.PointerType(TheBool, 0);
        
        // Compile `Bool.And`

        string funcName = "Bool.And..Bool";
        LLVMValueRef func = LLVM.GetNamedFunction(_module, funcName);
        if (func.Pointer != IntPtr.Zero)
        {
            // Unexpected: function is already declared
            if (LLVM.CountBasicBlocks(func) != 0)
            {
                throw new LLVMGenException("Function with name " + funcName + " already has a body");
            }
        }

        var parames = new LLVMTypeRef[]
        {
            /*this*/PBool,
            /*other*/PBool
        };

        func = LLVM.AddFunction(_module, funcName, LLVM.FunctionType(PBool, parames, false));
        LLVM.SetLinkage(func, LLVMLinkage.LLVMExternalLinkage);
        LLVM.SetValueName(LLVM.GetParam(func, 0), "this");
        LLVM.SetValueName(LLVM.GetParam(func, 1), "other");
        
        LLVM.PositionBuilderAtEnd(_builder, LLVM.AppendBasicBlock(func, "entry"));
        LLVMValueRef pu1 = LLVM.BuildStructGEP(_builder, LLVM.GetParam(func, 0), 0, "preunbox1");
        LLVMValueRef pu2 = LLVM.BuildStructGEP(_builder, LLVM.GetParam(func, 1), 0, "preunbox2");
        LLVMValueRef u1 = LLVM.BuildLoad(_builder, pu1, "unbox1");
        LLVMValueRef u2 = LLVM.BuildLoad(_builder, pu2, "unbox2");
        LLVMValueRef res = LLVM.BuildAnd(_builder, u1, u2, "res");
        LLVMValueRef resptr = LLVM.BuildMalloc(_builder, TheBool, "resptr");
        LLVMValueRef resPacked = LLVM.BuildAlloca(_builder, TheBool, "resPacked");
        LLVM.BuildStore(_builder, res, LLVM.BuildStructGEP(_builder, resPacked, 0, ""));
        LLVM.BuildStore(_builder, LLVM.BuildLoad(_builder, resPacked, ""), resptr);
        
        LLVM.BuildRet(_builder, resptr);

        LLVM.VerifyFunction(func, LLVMVerifierFailureAction.LLVMPrintMessageAction);

        // Compile `Bool.TerminateExecution`

        funcName = "Bool.TerminateExecution..";
        func = LLVM.GetNamedFunction(_module, funcName);
        if (func.Pointer != IntPtr.Zero)
        {
            // Unexpected: function is already declared
            if (LLVM.CountBasicBlocks(func) != 0)
            {
                throw new LLVMGenException("Function with name " + funcName + " already has a body");
            }
        }

        parames = new LLVMTypeRef[]
        {
            /*this*/PBool
        };

        func = LLVM.AddFunction(_module, funcName, LLVM.FunctionType(PBool, parames, false));
        LLVM.SetLinkage(func, LLVMLinkage.LLVMExternalLinkage);
        LLVM.SetValueName(LLVM.GetParam(func, 0), "this");

        LLVM.PositionBuilderAtEnd(_builder, LLVM.AppendBasicBlock(func, "entry"));
        var unboxed = LLVM.BuildStructGEP(_builder, LLVM.GetParam(func, 0), 0, "unboxed");
        LLVM.BuildCall(_builder, exit,
            new[] { LLVM.BuildIntCast(_builder, LLVM.BuildLoad(_builder, unboxed, ""), LLVM.Int32Type(), "") }, "");
        LLVM.BuildRet(_builder, LLVM.ConstNull(PBool));
        LLVM.VerifyFunction(func, LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }

    public void CheckAndDump()
    {
        LLVM.DumpModule(_module);
        LLVM.VerifyModule(_module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out var err);
    }

    public void Compile(TypesAwareProgram program)
    {
        LLVMTypeRef TheBool = LLVM.StructType(new[] { LLVM.Int1Type() }, false);
        LLVMTypeRef Ptr = LLVM.PointerType(TheBool, 0);  /* In up-to-date LLVM versions pointers are generic, the
                                                          * pointee type does not matter. */

        foreach (ITypesAwareClassDeclaration cls in program.ClassDeclarations())
        {
            DeclareClass(cls);
        }
        foreach (ITypesAwareClassDeclaration cls in program.ClassDeclarations())
        {
            CompileClass(cls);
        }
        
        // Compile _CO_entrypoint. This function shall be the entry point of ChineseObjects' runtime
        LLVMValueRef main = LLVM.GetNamedFunction(_module, "Main.Main..");
        if (main.Pointer == IntPtr.Zero)
        {
            throw new LLVMGenException("Could not find class Main with method Main()");
        }
        
        LLVMValueRef co_entry = LLVM.AddFunction(_module, "_CO_entrypoint",
            LLVM.FunctionType(LLVM.VoidType(), new LLVMTypeRef[] { }, false));
        LLVM.SetLinkage(co_entry, LLVMLinkage.LLVMExternalLinkage);
        LLVM.PositionBuilderAtEnd(_builder, LLVM.AppendBasicBlock(co_entry, "entry"));
        // TODO: first construct an object of type `Main` and call the method with that value rather than a null pointer
        LLVM.BuildCall(_builder, main, new LLVMValueRef[] { LLVM.ConstNull(Ptr) }, "");
        LLVM.BuildRetVoid(_builder);
    }

    private void DeclareClass(ITypesAwareClassDeclaration cls)
    {
        LLVMTypeRef TheBool = LLVM.StructType(new[] { LLVM.Int1Type() }, false);
        LLVMTypeRef Ptr = LLVM.PointerType(TheBool, 0);  /* In up-to-date LLVM versions pointers are generic, the
                                                          * pointee type does not matter. */

        
        // TODO: Declare the type with the appropriate name and the right number of pointers

        
        foreach (ITypesAwareMethod method in cls.MethodDeclarations())
        {
            string funcName = cls.ClassName() + '.' + method.MethodName() + ".." +
                              String.Join('.', method.Parameters().GetParameters().Select(x => x.Name()));
            string retName = method.ReturnType().TypeName().Value();
            LLVMTypeRef retT = Ptr;  // TODO: use the right return name based on `retName`
            LLVMValueRef func = LLVM.AddFunction(_module, funcName,
                LLVM.FunctionType(retT, Enumerable.Repeat(Ptr, 1+method.Parameters().GetParameters().Count()).ToArray(),
                    false));
            LLVM.SetLinkage(func, LLVMLinkage.LLVMExternalLinkage);

            uint i = 0;
            LLVM.SetValueName(LLVM.GetParam(func, 0), "this");
            foreach (ITypedParameter param in method.Parameters().GetParameters())
            {
                ++i;
                LLVM.SetValueName(LLVM.GetParam(func, i), param.Name());
            }
        }
    }

    private void CompileClass(ITypesAwareClassDeclaration cls)
    {
        LLVMTypeRef TheBool = LLVM.StructType(new[] { LLVM.Int1Type() }, false);
        LLVMTypeRef Ptr = LLVM.PointerType(TheBool, 0);  /* In up-to-date LLVM versions pointers are generic, the
                                                          * pointee type does not matter. */
        // TODO 1: compile methods
        // TODO 2: compile constructors
        // TODO 3: support inheritance

        foreach (ITypesAwareMethod method in cls.MethodDeclarations())
        {
            string funcName = cls.ClassName() + '.' + method.MethodName() + ".." +
                              String.Join('.', method.Parameters().GetParameters().Select(x => x.Name()));
            LLVMValueRef func = LLVM.GetNamedFunction(_module, funcName);
            LLVM.PositionBuilderAtEnd(_builder, LLVM.AppendBasicBlock(func, "entry"));
            foreach (ITypesAwareStatement statement in method.Body().Statements())
            {
                // Build the statement. It is built and inserted to the builder position, which should remain
                // at the end of the function after every statement is built.
                statement.AcceptVisitor(this);
            }

            // Return a nil reference in case the execution did not reach a return statement.
            LLVM.BuildRet(_builder, LLVM.ConstNull(Ptr));
        }
    }

    // TODO: remove this method
    public LLVMValueRef AddTrivialMethod(string className, string methodName, LLVMValueRef doubleToReturn)
    {
        string funcName = className + "::" + methodName;
        LLVMValueRef func = LLVM.GetNamedFunction(_module, funcName);
        if (func.Pointer != IntPtr.Zero)
        {
            // Unexpected: function is already declared
            if (LLVM.CountBasicBlocks(func) != 0)
            {
                throw new LLVMGenException("Function with name " + funcName + " already has a body");
            }
        }
        // TODO: build `LLVMTypeRef`s for types. For now, using argless functions that return doubles
        func = LLVM.AddFunction(_module, funcName,
            LLVM.FunctionType(LLVM.DoubleType(), Array.Empty<LLVMTypeRef>(), false));
        LLVM.SetLinkage(func, LLVMLinkage.LLVMExternalLinkage);
        // set params names here

        LLVM.PositionBuilderAtEnd(_builder, LLVM.AppendBasicBlock(func, "entry"));
        LLVM.BuildRet(_builder, doubleToReturn);

        LLVM.VerifyFunction(func, LLVMVerifierFailureAction.LLVMPrintMessageAction);

        func = LLVM.GetNamedFunction(_module, funcName);
        return func;
    }
    
    public LLVMValueRef Visit(TypedBoolLiteral boolLit)
    {
        // TODO: refer to the struct by name...
        LLVMTypeRef Bool = LLVM.StructType(new[] { LLVM.Int1Type() }, false);
        LLVMValueRef boxed = LLVM.BuildMalloc(_builder, Bool, "boxed");
        LLVMValueRef direct = LLVM.BuildStructGEP(_builder, boxed, 0, "direct");
        LLVM.BuildStore(_builder, LLVM.ConstInt(LLVM.Int1Type(), boolLit.Value() ? 1UL : 0, false), direct);
        return boxed;
    }

    public LLVMValueRef Visit(TypedNumLiteral numLit)
    {
        throw new NotImplementedException();
        // Should be boxed (allocated on heap)
        return LLVM.ConstReal(LLVM.DoubleType(), numLit.Value());
    }

    public LLVMValueRef Visit(TypedMethodCall methodCall)
    {
        string funcName = methodCall.Caller().Type().TypeName().Value() + "." + methodCall.MethodName() + ".." +
                          String.Join('.',
                              methodCall.Arguments().Values().Select(arg => arg.Value().Type().TypeName().Value()));
        LLVMValueRef func = LLVM.GetNamedFunction(_module, funcName);
        if (func.Pointer == IntPtr.Zero)
        {
            throw new LLVMGenException("Function " + funcName + " is not declared");
        }
        if (LLVM.CountBasicBlocks(func) == 0)
        {
            // TODO: Empty methods will trigger this exception. This is just a temporary check for while we're building
            // the compiler, later this whole check should be removed.
            throw new LLVMGenException("Function " + funcName + " has empty body. Was it ever defined?");
        }
        
        // Compile all arguments' values (other than "this")
        IEnumerable<LLVMValueRef> args = methodCall.Arguments().Values().Select(arg => arg.Value().AcceptVisitor(this));
        // The caller itself (aka "this") is an implicit first argument to every method call:
        args = args.Prepend(methodCall.Caller().AcceptVisitor(this));
        // Build the method call
        return LLVM.BuildCall(_builder, func, args.ToArray(), "");
    }

    public LLVMValueRef Visit(TypedParameter _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(TypedVariable _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(TypedArgument _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(TypedClassInstantiation _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(TypedReference _)
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
