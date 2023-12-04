namespace ChineseObjects.Lang;

public interface INativeEntityCompiler
{
    /// <summary>
    /// Compile a native entity (such as a type) in the context and module provided by `g`. Store the types of newly
    /// generated functions and structures in `g`.
    /// </summary>
    /// <param name="g">Native types code generator</param>
    public void CompileWith(CodeGen.LLVMExposingCodeGen g);
}
