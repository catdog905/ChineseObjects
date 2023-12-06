using LLVMSharp.Interop;

namespace ChineseObjects.Lang.Native;

/// <summary>
/// Declares function from the C library that ChineseObjects use.
/// </summary>
public class LibC : INativeEntityCompiler
{
    public void CompileWith(CodeGen.LLVMExposingCodeGen g)
    {
        var ctx = g.ctx;
        var module = g.module;
        var FuncType = g.FuncType;
        
        var mallocN = "malloc";
        var mallocT = FuncType[mallocN] =
            LLVMTypeRef.CreateFunction(LLVMTypeRef.CreatePointer(ctx.Int8Type, 0), new[] { ctx.Int32Type });
        var malloc = module.AddFunction(mallocN, mallocT);
        malloc.Linkage = LLVMLinkage.LLVMExternalLinkage;

        var exitN = "exit";
        var exitT = FuncType[exitN] = LLVMTypeRef.CreateFunction(ctx.VoidType, new[] { ctx.Int32Type });
        var exit = module.AddFunction(exitN, exitT);
        exit.Linkage = LLVMLinkage.LLVMExternalLinkage;

        var printfN = "printf";
        var printfT = FuncType[printfN] = LLVMTypeRef.CreateFunction(ctx.Int32Type, new[] { g.OpaquePtr }, true);
        var printf = module.AddFunction(printfN, printfT);
        printf.Linkage = LLVMLinkage.LLVMExternalLinkage;
    }
}
