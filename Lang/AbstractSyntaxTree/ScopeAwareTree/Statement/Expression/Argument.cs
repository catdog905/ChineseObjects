using System.Collections.Immutable;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;

public interface IScopeAwareArgument : IScopeAwareAstNode
{
    public IScopeAwareExpression Value();
}

public interface IScopeAwareArguments : IScopeAwareExpression
{
    public IEnumerable<IScopeAwareArgument> Values();
}

public class ScopeAwareArgument : IScopeAwareArgument
{
    private readonly Scope _scope;
    private readonly IScopeAwareExpression _value;

    private ScopeAwareArgument(Scope scope, IScopeAwareExpression value)
    {
        _scope = scope;
        _value = value;
    }

    public ScopeAwareArgument(Scope scope, IArgument argument)
        : this(scope, Irrealizable.MakeScopeAware(scope, argument.Value())) {}

    public Scope Scope()
    {
        return _scope;
    }

    public IScopeAwareExpression Value()
    {
        return _value;
    }

}

public class ScopeAwareArguments : IScopeAwareArguments
{
    private readonly Scope _scope;
    private readonly IEnumerable<IScopeAwareArgument> _values;

    public ScopeAwareArguments(Scope scope, IEnumerable<IScopeAwareArgument> arguments)
    {
        _scope = scope;
        _values = arguments.ToImmutableList();
    }

    public ScopeAwareArguments(Scope scope, IArguments arguments)
        : this(
            scope,
            arguments.Values().Select(argument => new ScopeAwareArgument(scope, argument))) { }


    public Scope Scope()
    {
        return _scope;
    }

    public IEnumerable<IScopeAwareArgument> Values()
    {
        return _values;
    }
}
