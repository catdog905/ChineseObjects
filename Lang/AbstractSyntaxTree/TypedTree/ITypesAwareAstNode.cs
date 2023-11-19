namespace ChineseObjects.Lang;

public interface ITypesAwareAstNode : IAstNode {}

public interface ITypedAstNode : IAstNode
{
    public Type Type();
}