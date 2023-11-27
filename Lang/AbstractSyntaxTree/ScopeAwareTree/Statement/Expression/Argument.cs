using System.Collections.Immutable;

namespace ChineseObjects.Lang
{
    public interface IScopeAwareArgument : IScopeAwareExpression
    {
        public IScopeAwareExpression Value();
    }

    public interface IScopeAwareArguments : IScopeAwareExpression
    {
        public IEnumerable<IScopeAwareArgument> Values();
    }

    public class ScopeAwareArgument : IScopeAwareArgument
    {
        private readonly Scope _scope;
        private readonly IScopeAwareExpression _value;

        public ScopeAwareArgument(Scope scope, IScopeAwareExpression value)
        {
            _scope = scope;
            _value = value;
        }

        public Scope Scope()
        {
            return _scope;
        }

        public IScopeAwareExpression Value()
        {
            return _value;
        }

    }

    public class ScopeAwareArguments : IScopeAwareArguments
    {
        private readonly Scope _scope;
        private readonly ImmutableList<IScopeAwareArgument> _values;

        public ScopeAwareArguments(Scope scope, IEnumerable<IScopeAwareArgument> arguments)
        {
            _scope = scope;
            _values = arguments.ToImmutableList();
        }

        public ScopeAwareArguments(Scope scope, params IScopeAwareArgument[] arguments)
        : this(scope, arguments.ToImmutableList())
        {}

        public ScopeAwareArguments(Scope scope, ScopeAwareArguments arguments, IScopeAwareArgument argument) 
        : this(scope, arguments._values.Add(argument)) { }

        public ScopeAwareArguments(Scope scope, IScopeAwareArgument argument, ScopeAwareArguments arguments) 
        : this(scope, new[] { argument }.Concat(arguments._values)) { }


        public Scope Scope()
        {
            return _scope;
        }

        public IEnumerable<IScopeAwareArgument> Values()
        {
            return _values;
        }
    }
}
