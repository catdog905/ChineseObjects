namespace ChineseObjects.Lang;

public interface IAstTree
{
    public IDeclarationAstNode Root();
}

public class SyntacticAstTree : IAstTree
{
    private readonly IProgramDeclaration _root;

    public SyntacticAstTree(IProgramDeclaration root)
    {
        _root = root;
    }

    public IDeclarationAstNode Root()
    {
        return _root;
    }
}

public class SemanticDeclarationAstTree : IDeclarationAstNode
{
    private readonly IScopeAwareProgram _root;

    public SemanticDeclarationAstTree(IScopeAwareProgram program)
    {
        _root = program;
    }
}
