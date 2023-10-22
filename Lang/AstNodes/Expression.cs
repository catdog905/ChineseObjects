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
    
}
