namespace ChineseObjects.Lang
{
    public interface IScopeAwareAssignment : IScopeAwareStatement
    {
        public IScopeAwareIdentifier VariableName();
        public IScopeAwareExpression Expression();
        public IScopeAwareIdentifier TypeName();
    }

    public class ScopeAwareAssignment : IScopeAwareAssignment
    {
        private readonly Scope _scope;
        private readonly IScopeAwareIdentifier _varName;
        private readonly IScopeAwareExpression _expr;
        private readonly IScopeAwareIdentifier _typeName;

        public ScopeAwareAssignment(
            Scope scope, 
            IScopeAwareIdentifier varName,
            IScopeAwareExpression expr,
            IScopeAwareIdentifier typeName)
        {
            _scope = scope;
            _varName = varName;
            _expr = expr;
            _typeName = typeName;
        }

        public ScopeAwareAssignment(Scope scope, IAssignment assignment) :
            this(
                scope, 
                new ScopeAwareIdentifier(scope, assignment.Name()),
                Irrealizable.MakeScopeAware(scope, assignment.Expr()),
                new ScopeAwareIdentifier(scope, assignment.TypeName()))
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

        public IScopeAwareIdentifier TypeName()
        {
            return _typeName;
        }
    }
}
