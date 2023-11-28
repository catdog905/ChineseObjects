namespace ChineseObjects.Lang;

public class Irrealizable
{
    public static IScopeAwareExpression MakeScopeAware(Scope scope, IExpression expression)
    {
        return expression switch
        {
            IArgument argument => new ScopeAwareArgument(scope, argument),
            IBoolLiteral boolLiteral => new ScopeAwareBoolLiteral(scope, boolLiteral),
            IClassInstantiation classInstantiation => new ScopeAwareClassInstantiation(scope, classInstantiation),
            Reference reference => new ScopeAwareReference(scope, reference),
            IMethodCall methodCall => new ScopeAwareMethodCall(scope, methodCall),
            INumLiteral numLiteral => new ScopeAwareNumLiteral(scope, numLiteral),
            _ => throw new NotImplementedException("Implementation of expression not found")
        };
    }

    public static IScopeAwareStatement MakeScopeAware(Scope scope, IStatement statement)
    {
        return statement switch
        {
            IAssignment assignment => new ScopeAwareAssignment(scope, assignment),
            IIfElse ifElse => new ScopeAwareIfElse(scope, ifElse),
            IReturn @return => new ScopeAwareReturn(scope, @return),
            IWhile @while => new ScopeAwareWhile(scope, @while),
            IExpression expression => MakeScopeAware(scope, expression),
            _ => throw new NotImplementedException("Implementation of statement not found")
        };
    }
}
