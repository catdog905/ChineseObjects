namespace ChineseObjects.Lang;

public interface IAstTree
{
    public IAstNode Root();
}

public class SyntacticAstTree : IAstTree
{
    private readonly IProgram _root;

    public SyntacticAstTree(IProgram root)
    {
        _root = root;
    }

    public IAstNode Root()
    {
        return _root;
    }
}

public class SemanticAstTree : IAstNode
{
    private readonly IScopeAwareProgram _root;

    public SemanticAstTree(IScopeAwareProgram program)
    {
        _root = program;
    }
}
