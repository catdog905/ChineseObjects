namespace ChineseObjects.Lang;

public interface IBoolLiteralDeclaration : IBoolLiteral, IExpressionDeclaration
{
    public bool Value();
}

// The boolean literal expression
// TODO: merge with `NumLiteral`?
public class BoolLiteral : IBoolLiteralDeclaration {
    private readonly bool _value;

    public BoolLiteral(bool value) {
        _value = value;
    }

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