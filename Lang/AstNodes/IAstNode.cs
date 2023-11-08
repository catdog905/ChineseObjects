namespace ChineseObjects.Lang;

public interface IAstNode {}


public class ScopeAwareAstNode : IAstNode
{
    public readonly IAstNode IAstNode;

    public ScopeAwareAstNode(IAstNode astNode)
    {
        IAstNode = astNode;
    }
}