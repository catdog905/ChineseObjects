using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;
using TypedReference = ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression.TypedReference;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree;

public class TypeIrrealizable
{
    public static ITypedExpression MakeTypedExpression(IScopeAwareExpression expression)
    {
        return expression switch
        {
            IScopeAwareBoolLiteral boolLiteral => new TypedBoolLiteral(boolLiteral),
            IScopeAwareClassInstantiation classInstantiation => new TypedClassInstantiation(classInstantiation),
            IScopeAwareReference identifier => new TypedReference(identifier),
            IScopeAwareMethodCall methodCall => new TypedMethodCall(methodCall),
            IScopeAwareNumLiteral numLiteral => new TypedNumLiteral(numLiteral),
            IScopeAwareThis @this => new Declaration.TypedThis(@this),
            _ => throw new NotImplementedException("Implementation of expression not found")
        };
    }

    public static ITypesAwareStatement MakeTypesAwareStatement(IScopeAwareStatement statement)
    {
        return statement switch
        {
            IScopeAwareAssignment assignment => new TypesAwareAssignment(assignment),
            IScopeAwareIfElse ifElse => new TypesAwareIfElse(ifElse),
            IScopeAwareReturn @return => new TypesAwareReturn(@return),
            IScopeAwareWhile @while => new TypesAwareWhile(@while),
            IScopeAwareExpression expression => MakeTypedExpression(expression),
            _ => throw new NotImplementedException("Implementation of statement not found")
        };
    }
}
