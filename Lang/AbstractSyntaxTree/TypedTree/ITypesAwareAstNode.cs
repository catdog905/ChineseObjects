namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree;

public interface ITypesAwareAstNode {}

public interface ITypedAstNode : ITypesAwareAstNode
{
    public Type Type();
}