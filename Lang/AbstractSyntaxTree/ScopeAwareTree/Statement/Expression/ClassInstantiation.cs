namespace ChineseObjects.Lang
{
    public interface IScopeAwareClassInstantiation : IScopeAwareExpression
    {
        public IScopeAwareIdentifier ClassName();
        public IScopeAwareArguments Arguments();
    }

    public class ScopeAwareClassInstantiation : IScopeAwareClassInstantiation
    {
        private readonly Scope _scope;
        private readonly IScopeAwareIdentifier _className;
        private readonly IScopeAwareArguments _arguments;

        public ScopeAwareClassInstantiation(
            Scope scope, 
            IScopeAwareIdentifier className, 
            IScopeAwareArguments arguments)
        {
            _scope = scope;
            _className = className;
            _arguments = arguments;
        }

        public ScopeAwareClassInstantiation(Scope scope, IClassInstantiation classInstantiation)
            : this(
                scope, 
                new ScopeAwareIdentifier(scope, classInstantiation.ClassName()), 
                new ScopeAwareArguments(scope, classInstantiation.Arguments())) {}

        public Scope Scope()
        {
            return _scope;
        }

        public IScopeAwareIdentifier ClassName()
        {
            return _className;
        }

        public IScopeAwareArguments Arguments()
        {
            return _arguments;
        }
    }
}
