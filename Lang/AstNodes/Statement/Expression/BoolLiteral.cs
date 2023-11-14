namespace ChineseObjects.Lang;

// The boolean literal expression
// TODO: merge with `NumLiteral`?
public class BoolLiteral : Expression {
    public readonly bool value;

    public BoolLiteral(bool value) {
        this.value = value;
    }

    public ClassDeclaration? EvaluatedType(Scope scope) {
        throw new NotImplementedException();
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
