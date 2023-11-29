using ChineseObjects.Lang.Declaration;
using LLVMSharp;

namespace ChineseObjects.Lang.CodeGen;

public class LLVMGen : ITypedNodeVisitor<LLVMValueRef>
{
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

    public LLVMValueRef Visit(TypedBoolLiteral _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(TypedClassInstantiation _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(TypedMethodCall _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(TypedNumLiteral _)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Visit(TypedReference _)
    {
        throw new NotImplementedException();
    }
}