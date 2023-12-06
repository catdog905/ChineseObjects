namespace ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree;

public interface IScopeAwareAstNode
{
    public Scope Scope();
}