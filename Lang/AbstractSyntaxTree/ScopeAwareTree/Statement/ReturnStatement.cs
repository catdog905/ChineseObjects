using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement;
   
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

    public ScopeAwareReturn(Scope scope, IReturn returnValue) :
        this(scope, Irrealizable.MakeScopeAware(scope, returnValue.Value())) {}

    public Scope Scope()
    {
        return _scope;
    }

    public IScopeAwareExpression ReturnValue()
    {
        return _returnValue;
    }

}
