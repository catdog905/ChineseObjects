using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

public interface INumLiteral : IExpression
{
    public double Value();
}

// The literal number expression (both for integers and floats)
// TODO: separate integers and floats? Leave as is?
public class NumLiteral : INumLiteral {
    private readonly double _value;

    public NumLiteral(double value) {
        _value = value;
    }

    public NumLiteral(ITypedNumLiteral numLiteral) :
        this(numLiteral.Value()) {}

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