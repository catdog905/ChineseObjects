

namespace ChineseObjects.Lang;

public interface IScopeAwareIfElse : IIfElseDeclaration, IScopeAwareStatementsBlock {}

public class ScopeAwareIfElse : IScopeAwareIfElse
{
    private readonly Scope _scope;
    private readonly IExpressionDeclaration _cond;
    private readonly IScopeAwareStatementsBlock _then;
    private readonly IScopeAwareStatementsBlock? _else;

    public ScopeAwareIfElse(Scope scope, IExpressionDeclaration cond, IScopeAwareStatementsBlock then, IScopeAwareStatementsBlock? else_)
    {
        _scope = scope;
        _cond = cond;
        _then = then;
        _else = else_;
    }

    public Scope Scope()
    {
        return _scope;
    }

    public IExpressionDeclaration Condition()
    {
        return _cond;
    }

    public IScopeAwareStatementsBlock ThenBlock()
    {
        return _then;
    }

    public IScopeAwareStatementsBlock? ElseBlock()
    {
        return _else;
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
    IList<string> IHumanReadable.GetRepr()
    {
        throw new NotImplementedException();
    }

}
