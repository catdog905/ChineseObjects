using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareIfElse : IIfElse, IScopeAwareStatementsBlock {}

public class ScopeAwareIfElse : IScopeAwareIfElse
{
    private readonly Scope _scope;
    private readonly IExpression _cond;
    private readonly IScopeAwareStatementsBlock _then;
    private readonly IScopeAwareStatementsBlock? _else;

    public ScopeAwareIfElse(Scope scope, IExpression cond, IScopeAwareStatementsBlock then, IScopeAwareStatementsBlock? else_)
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

    public IExpression Condition()
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
    public IEnumerable<IStatement> Statements()
    {
        return ImmutableList<IStatement>.Empty.Add(_cond)
            .AddRange(_then.Statements())
            .AddRange(_else.Statements());
    }

    IList<string> IHumanReadable.GetRepr()
    {
        throw new NotImplementedException();
    }

}
