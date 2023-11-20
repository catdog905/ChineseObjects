namespace ChineseObjects.Lang;

public interface IScopeAwareWhile : IWhile, IScopeAwareStatementsBlock {}

public class ScopeAwareWhile : IScopeAwareWhile
{
    private readonly Scope _scope;
    private readonly IExpressionDeclaration _cond;
    private readonly IScopeAwareStatementsBlock _body;

    public ScopeAwareWhile(Scope scope, IExpressionDeclaration condition, IScopeAwareStatementsBlock body)
    {
        _scope = scope;
        _cond = condition;
        _body = body;
    }

    public Scope Scope()
    {
        return _scope;
    }

    public IExpressionDeclaration Condition()
    {
        return _cond;
    }

    public IScopeAwareStatementsBlock Body()
    {
        return _body;
    }

    //TODO: Could be a need for a proper interface methods implementation 
    public IEnumerable<IStatementDeclaration> Statements()
    {
        throw new NotImplementedException();
    }

    IEnumerable<IStatement> IStatementsBlock.Statements()
    {
        return Statements();
    }
}
