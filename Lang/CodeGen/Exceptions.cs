namespace ChineseObjects.Lang.CodeGen;

// TODO: derive more classes to describe exceptions
public class LLVMGenException : Exception
{
    public LLVMGenException() : base() {}
    public LLVMGenException(string s) : base(s) {}
}
