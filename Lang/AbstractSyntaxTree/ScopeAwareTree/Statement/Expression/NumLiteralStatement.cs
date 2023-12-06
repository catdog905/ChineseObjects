using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;

public interface IScopeAwareNumLiteral : IScopeAwareExpression
{
    public double Value();
}

public class ScopeAwareNumLiteral : IScopeAwareNumLiteral
{
    private readonly Scope _scope;
    private readonly double _value;

    public ScopeAwareNumLiteral(Scope scope, double value)
    {
        _scope = scope;
        _value = value;
    }

    public ScopeAwareNumLiteral(Scope scope, INumLiteral numLiteral) :
        this(scope, numLiteral.Value()) {}

    public Scope Scope()
    {
        return _scope;
    }

    public double Value()
    {
        return _value;
    }
}
