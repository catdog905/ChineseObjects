namespace ChineseObjects.Lang;

public interface IScopeAwareWhile : IWhile, IScopeAwareStatementsBlock {}

public class ScopeAwareWhile : IScopeAwareWhile
{
    private readonly Scope _scope;
    private readonly IExpression _cond; // Should be ScopeAwareExpression?
    private readonly IScopeAwareStatementsBlock _body;

    public ScopeAwareWhile(Scope scope, IExpression condition, IScopeAwareStatementsBlock body)
    {
        _scope = scope;
        _cond = condition;
        _body = body;
    }

    public Scope Scope()
    {
        return _scope;
    }

    public IExpression Condition()
    {
        return _cond;
    }

    public IScopeAwareStatementsBlock Body()
    {
        return _body;
    }

    //TODO: Could be a need for a proper interface methods implementation 
    public IEnumerable<IStatement> Statements()
    {
        throw new NotImplementedException();
    }

    IList<string> IHumanReadable.GetRepr()
    {
        throw new NotImplementedException();
    }

}
