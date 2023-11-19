namespace ChineseObjects.Lang;

public interface INumLiteralDeclaration : INumLiteral, IExpressionDeclaration
{
    public double Value();
}

// The literal number expression (both for integers and floats)
// TODO: separate integers and floats? Leave as is?
public class NumLiteral : INumLiteralDeclaration {
    private readonly double _value;

    public NumLiteral(double value) {
        _value = value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    public double Value()
    {
        return _value;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"LITERAL " + _value};
    }
}