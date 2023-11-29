namespace ChineseObjects.Lang;

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
