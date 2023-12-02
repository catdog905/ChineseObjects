using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

public interface ITypedNumLiteral : ITypedExpression
{
    public double Value();
}

public class TypedNumLiteral : ITypedNumLiteral
{
    private readonly Type _type;
    private readonly double _value;

    public TypedNumLiteral(Type type, double value)
    {
        _type = type;
        _value = value;
    }

    public TypedNumLiteral(IScopeAwareNumLiteral numLiteral) :
        this(numLiteral.Scope().GetType("Number"), numLiteral.Value()) {}


    public Type Type()
    {
        return _type;
    }

    public double Value()
    {
        return _value;
    }

    public IList<string> GetRepr()
    {
        return new DeclarationAwareTree.Statement.Expression.NumLiteral(this).GetRepr();
    }
}