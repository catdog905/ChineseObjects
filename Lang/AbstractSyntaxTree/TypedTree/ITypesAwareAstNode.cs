namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree;

public interface ITypesAwareAstNode : IHumanReadable {}

public interface ITypedAstNode : ITypesAwareAstNode
{
    public Type Type();
}