namespace ChineseObjects.Lang;

public interface IAstNode {}


public interface IScopeAwareAstNode
{
    public Scope Scope();
}