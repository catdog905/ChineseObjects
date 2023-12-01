namespace ChineseObjects.Lang;

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

    public T AcceptVisitor<T>(CodeGen.ITypesAwareStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public double Value()
    {
        return _value;
    }
}