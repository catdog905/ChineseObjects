namespace ChineseObjects.Lang
{
    public interface IScopeAwareBoolLiteral : IScopeAwareExpression
    {
        public bool Value();
    }

    public class ScopeAwareBoolLiteral : IScopeAwareBoolLiteral
    {
        private readonly Scope _scope;
        private readonly bool _value;

        public ScopeAwareBoolLiteral(Scope scope, bool value)
        {
            _scope = scope;
            _value = value;
        }

        public ScopeAwareBoolLiteral(Scope scope, IBoolLiteral boolLiteral)
            : this(scope, boolLiteral.Value()) {}

        public Scope Scope()
        {
            return _scope;
        }

        public bool Value()
        {
            return _value;
        }
    }
}
