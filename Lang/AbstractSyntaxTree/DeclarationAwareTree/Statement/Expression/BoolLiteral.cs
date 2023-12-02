using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

public interface IBoolLiteral : IExpression
{
    public bool Value();
}

// The boolean literal expression
// TODO: merge with `NumLiteral`?
public class BoolLiteral : IBoolLiteral {
    private readonly bool _value;

    public BoolLiteral(bool value) {
        _value = value;
    }

    public BoolLiteral(ITypedBoolLiteral boolLiteral) :
        this(boolLiteral.Value()) {}

    public override string ToString()
    {
        return _value.ToString();
    }

    public bool Value()
    {
        return _value;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"LITERAL " + _value.ToString()};
    }
}