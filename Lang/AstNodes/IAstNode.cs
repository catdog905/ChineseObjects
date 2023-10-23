namespace ChineseObjects.Lang;

public interface IAstNode
{ 
    List<IAstNode> Children();
    IAstNode CurrentNode();
}