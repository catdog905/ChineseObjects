namespace ChineseObjects.Lang;

public interface IScopeAwareWhile : IScopeAwareStatement
{
    public IScopeAwareExpression Condition();
    public IScopeAwareStatementsBlock Body();
}

public class ScopeAwareWhile : IScopeAwareWhile
{
    private readonly Scope _scope;
    private readonly IScopeAwareExpression _cond;
    private readonly IScopeAwareStatementsBlock _body;

    public ScopeAwareWhile(Scope scope, IScopeAwareExpression condition, IScopeAwareStatementsBlock body)
    {
        _scope = scope;
        _cond = condition;
        _body = body;
    }

    public ScopeAwareWhile(Scope scope, IWhile whileStmt)
    : this(
        scope, 
        Irrealizable.MakeScopeAware(scope, whileStmt.Condition()),
        new ScopeAwareStatementsBlock(scope, whileStmt.Body()))
    {}

    public Scope Scope()
    {
        return _scope;
    }

    public IScopeAwareExpression Condition()
    {
        return _cond;
    }

    public IScopeAwareStatementsBlock Body()
    {
        return _body;
    }
}
