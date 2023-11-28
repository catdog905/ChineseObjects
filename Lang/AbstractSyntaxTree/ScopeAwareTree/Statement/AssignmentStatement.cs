namespace ChineseObjects.Lang
{
    public interface IScopeAwareAssignment : IScopeAwareStatement
    {
        public IScopeAwareIdentifier VariableName();
        public IScopeAwareExpression Expression();
    }

    public class ScopeAwareAssignment : IScopeAwareAssignment
    {
        private readonly Scope _scope;
        private readonly IScopeAwareIdentifier _varName;
        private readonly IScopeAwareExpression _expr;

        public ScopeAwareAssignment(Scope scope, IScopeAwareIdentifier varName, IScopeAwareExpression expr)
        {
            _scope = scope;
            _varName = varName;
            _expr = expr;
        }

        public ScopeAwareAssignment(Scope scope, IAssignment assignment) :
            this(
                scope, 
                new ScopeAwareIdentifier(scope, assignment.Name()),
                Irrealizable.MakeScopeAware(scope, assignment.Expr()))
        {}

        public Scope Scope()
        {
            return _scope;
        }

        public IScopeAwareIdentifier VariableName()
        {
            return _varName;
        }

        public IScopeAwareExpression Expression()
        {
            return _expr;
        }
    }
}
