using LLVMSharp.Interop;

namespace ChineseObjects.Lang.CodeGen;

/// <summary>
/// A raw code generator that exposes its internals. It should be used to generate code for native types, then be handed
/// to `CompiledProgram` and never be used again.
/// </summary>
public class LLVMExposingCodeGen
{
    /*
     * All fields have the same meanings as in `CompiledProgram`
     */
    
    public LLVMContextRef ctx;
    public LLVMModuleRef module;
    public LLVMBuilderRef builder;

    public Dictionary<string, LLVMTypeRef> FuncType;
    public Dictionary<string, LLVMTypeRef> Struct;

    public readonly LLVMTypeRef OpaquePtr;

    public LLVMExposingCodeGen()
    {
        ctx = LLVMContextRef.Create();
        module = ctx.CreateModuleWithName("main_module");
        builder = ctx.CreateBuilder();

        FuncType = new();
        Struct = new();
        
        OpaquePtr = LLVMTypeRef.CreatePointer(ctx.Int1Type, 0);
    }
}
