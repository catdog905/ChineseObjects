using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;
public interface IScopeAwareMethodCall : IScopeAwareExpression
{
    public IScopeAwareExpression Caller();
    public IScopeAwareIdentifier MethodName();
    public IScopeAwareArguments Arguments();
}

public class ScopeAwareMethodCall : IScopeAwareMethodCall
{
    private readonly Scope _scope;
    private readonly IScopeAwareExpression _caller;
    private readonly IScopeAwareIdentifier _methodName;
    private readonly IScopeAwareArguments _arguments;

    public ScopeAwareMethodCall(
        Scope scope, 
        IScopeAwareExpression caller, 
        IScopeAwareIdentifier methodName, 
        IScopeAwareArguments arguments)
    {
        _scope = scope;
        _caller = caller;
        _methodName = methodName;
        _arguments = arguments;
    }

    public ScopeAwareMethodCall(Scope scope, IMethodCall methodCall) :
        this(
            scope, 
            Irrealizable.MakeScopeAware(
                scope, 
                methodCall.Caller()),
            new ScopeAwareIdentifier(scope, methodCall.MethodName()),
            new ScopeAwareArguments(scope, methodCall.Arguments())) {}

    public Scope Scope()
    {
        return _scope;
    }

    public IScopeAwareExpression Caller()
    {
        return _caller;
    }

    public IScopeAwareIdentifier MethodName()
    {
        return _methodName;
    }

    public IScopeAwareArguments Arguments()
    {
        return _arguments;
    }
}