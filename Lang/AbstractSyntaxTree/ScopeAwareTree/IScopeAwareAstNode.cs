namespace ChineseObjects.Lang;

public interface IScopeAwareAstNode : IAstNode
{
    public Scope Scope();
}