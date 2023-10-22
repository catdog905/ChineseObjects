namespace ChineseObjects.Lang {
    // The base class for all expressions
    public abstract class Expression {}


    
    // The literal number expression (both for integers and floats)
    // TODO: separate integers and floats? Leave as is?
    public class NumLiteral : Expression {
        public readonly double value;

        public NumLiteral(double value) {
            this.value = value;
        }
    }

    // The boolean literal expression
    // TODO: merge with `NumLiteral`?
    public class BoolLiteral : Expression {
        public readonly bool value;

        public BoolLiteral(bool value) {
            this.value = value;
        }
    }

    // An identifier. Note that it is used to express that an `Identifier`
    // is an `Expression`. In more complex expressions that include identifiers
    // (such as variable/method/class declaration, etc) identifier is stored
    // as a mere `string` rather than the `Identifier` object.
    public class Identifier : Expression {
        public readonly string name;

        public Identifier(string name) {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
