namespace ChineseObjects.Lang;

// The literal number expression (both for integers and floats)
// TODO: separate integers and floats? Leave as is?
public class NumLiteral : Expression {
    public readonly double value;

    public NumLiteral(double value) {
        this.value = value;
    }

    public override string ToString()
    {
        return value.ToString();
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"LITERAL " + value.ToString()};
    }
}