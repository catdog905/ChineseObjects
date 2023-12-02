using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

public interface ITypedBoolLiteral : ITypedExpression
{
    public bool Value();
}

public class TypedBoolLiteral : ITypedBoolLiteral
{
    private readonly Type _type;
    private readonly bool _value;

    public TypedBoolLiteral(Type type, bool value)
    {
        _type = type;
        _value = value;
    }

    public TypedBoolLiteral(IScopeAwareBoolLiteral boolLiteral) :
        this(new Type(boolLiteral.Scope(), "Boolean"), boolLiteral.Value()) {}


    public Type Type()
    {
        return _type;
    }

    public bool Value()
    {
        return _value;
    }
}