namespace ChineseObjects.Lang
{
    public interface IScopeAwareReturn : IScopeAwareStatement
    {
        public IScopeAwareExpression ReturnValue();
    }

    public class ScopeAwareReturn : IScopeAwareReturn
    {
        private readonly Scope _scope;
        private readonly IScopeAwareExpression _returnValue;

        public ScopeAwareReturn(Scope scope, IScopeAwareExpression returnValue)
        {
            _scope = scope;
            _returnValue = returnValue;
        }

        public Scope Scope()
        {
            return _scope;
        }

        public IScopeAwareExpression ReturnValue()
        {
            return _returnValue;
        }

    }
}