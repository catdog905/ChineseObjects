using System.Security.Cryptography.X509Certificates;
using ChineseObjects.Lang.Declaration;
using LLVMSharp;

namespace ChineseObjects.Lang.CodeGen;

public class LLVMCodeGen : ITypedNodeVisitor<LLVMValueRef>
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

        funcName = "Bool.TerminateExecution.";
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

        func = LLVM.AddFunction(_module, funcName, LLVM.FunctionType(LLVMTypeRef.VoidType(), parames, false));
        LLVM.SetLinkage(func, LLVMLinkage.LLVMExternalLinkage);
        LLVM.SetValueName(LLVM.GetParam(func, 0), "this");

        LLVM.PositionBuilderAtEnd(_builder, LLVM.AppendBasicBlock(func, "entry"));
        var unboxed = LLVM.BuildStructGEP(_builder, LLVM.GetParam(func, 0), 0, "unboxed");
        LLVM.BuildCall(_builder, exit,
            new[] { LLVM.BuildIntCast(_builder, LLVM.BuildLoad(_builder, unboxed, ""), LLVM.Int32Type(), "") }, "");
        LLVM.BuildRetVoid(_builder);
        LLVM.VerifyFunction(func, LLVMVerifierFailureAction.LLVMPrintMessageAction);
        
        // Some final stuff
        
        LLVM.VerifyModule(_module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out var err);
        LLVM.DumpModule(_module);
    }

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
        LLVM.BuildStore(_builder, LLVM.ConstReal(LLVM.Int1Type(), boolLit.Value() ? 1 : 0), boxed);
        return boxed;
    }

    public LLVMValueRef Visit(TypedNumLiteral numLit)
    {
        return LLVM.ConstReal(LLVM.DoubleType(), numLit.Value());
    }

    public LLVMValueRef Visit(TypedMethodCall methodCall)
    {
        string funcName = methodCall.Caller().Type().TypeName().Value() + "::" + methodCall.MethodName();
        LLVMValueRef func = LLVM.GetNamedFunction(_module, funcName);
        if (func.Pointer == IntPtr.Zero)
        {
            throw new LLVMGenException("Function " + funcName + " is not defined");
        }
        
        // TODO: deduce type for each of the arguments: `methodCall.Arguments().Values(). [for each] .Type();`
        // TODO: "this" is an implicit first argument to every method call!
        var args = methodCall.Arguments().Values();
        var refArgs = new LLVMValueRef[args.Count()];
        return LLVM.BuildCall(_builder, func, refArgs, "result_of/" + funcName);
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
}
