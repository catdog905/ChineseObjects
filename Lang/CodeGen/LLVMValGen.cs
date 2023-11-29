using ChineseObjects.Lang.Declaration;
using LLVMSharp;

namespace ChineseObjects.Lang.CodeGen;

public class LLVMValGen : ITypedNodeVisitor<LLVMValueRef>
{
    private readonly LLVMModuleRef _module;
    private readonly LLVMBuilderRef _builder;

    public LLVMValGen()
    {
        _module = LLVM.ModuleCreateWithName("this");
        _builder = LLVM.CreateBuilder();
    }
    
    public LLVMValueRef Visit(TypedBoolLiteral boolLit)
    {
        return LLVM.ConstReal(LLVM.Int1Type(), boolLit.Value() ? 1 : 0);
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
        return LLVM.BuildCall(_builder, func, refArgs, "call/" + func);
    }

    /*
    something for method declaration
    {
        string funcName = methodCall.Caller().Type().TypeName().Value() + "::" + methodCall.MethodName();
        LLVMValueRef func = LLVM.GetNamedFunction(_module, funcName);
        if (func.Pointer != IntPtr.Zero)
        {
            // Unexpected: function is already declared
            if (LLVM.CountBasicBlocks(func) != 0)
            {
                throw new LLVMGenException("Function with name " + funcName + " already has a body");
            }
        }
        else
        {
            // TODO: build `LLVMTypeRef`s for types. For now, using argless functions that return doubles
            func = LLVM.AddFunction(_module, funcName,
                LLVM.FunctionType(LLVM.DoubleType(), Array.Empty<LLVMTypeRef>(), false));
            LLVM.SetLinkage(func, LLVMLinkage.LLVMExternalLinkage);
            // set params names here

            LLVM.PositionBuilderAtEnd(_builder, LLVM.AppendBasicBlock(func, "entry"));
            
            // TODO: add actual blocks
        }

        return func;
    }
    */

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
