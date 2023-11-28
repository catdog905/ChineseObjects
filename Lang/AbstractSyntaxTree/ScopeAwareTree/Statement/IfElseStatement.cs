using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareIfElse : IScopeAwareStatement
{
    public IScopeAwareExpression Condition();
    public IScopeAwareStatementsBlock ThenBlock();
    public IScopeAwareStatementsBlock ElseBlock();
}

public class ScopeAwareIfElse : IScopeAwareIfElse
{
    private readonly Scope _scope;
    private readonly IScopeAwareExpression _cond;
    private readonly IScopeAwareStatementsBlock _then;
    private readonly IScopeAwareStatementsBlock? _else;

    public ScopeAwareIfElse(Scope scope, IScopeAwareExpression cond, IScopeAwareStatementsBlock then, IScopeAwareStatementsBlock? else_)
    {
        _scope = scope;
        _cond = cond;
        _then = then;
        _else = else_;
    }

    public ScopeAwareIfElse(Scope scope, IIfElse ifElseStmt)
    : this (scope, Irrealizable.MakeScopeAware(scope, ifElseStmt.Condition()),
        new ScopeAwareStatementsBlock(scope, ifElseStmt.Then()),
        ifElseStmt.Else() is null ? null : new ScopeAwareStatementsBlock(scope, ifElseStmt.Else()))
    {}

    public Scope Scope()
    {
        return _scope;
    }

    public IScopeAwareExpression Condition()
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

    //TODO: Either remove this method or add to one of the interfaces     
    public IEnumerable<IScopeAwareStatement> Statements()
    {
        return ImmutableList<IScopeAwareStatement>.Empty.Add(_cond)
            .AddRange(_then.Statements())
            .AddRange(_else?.Statements() ?? ImmutableList<IScopeAwareStatement>.Empty);
    }
}
