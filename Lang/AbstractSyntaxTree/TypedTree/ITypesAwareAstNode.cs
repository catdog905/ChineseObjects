namespace ChineseObjects.Lang;

public interface ITypesAwareAstNode {}

public interface ITypedAstNode
{
    public Type Type();
}