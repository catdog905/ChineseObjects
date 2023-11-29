namespace ChineseObjects.Lang;

public interface ITypesAwareAstNode {}

public interface ITypedAstNode
{
    public Type Type();
    public T AcceptVisitor<T>(CodeGen.ITypedNodeVisitor<T> visitor);
}
