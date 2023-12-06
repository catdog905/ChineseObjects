using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration.Parameter;

namespace ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Declaration.Parameter;

public interface IScopeAwareParameters : IScopeAwareAstNode
{
    public IEnumerable<IScopeAwareParameter> GetParameters();
}

public class ScopeAwareParameters : IScopeAwareParameters
{
    private readonly Scope _scope;
    private readonly IEnumerable<IScopeAwareParameter> _parameters;

    public ScopeAwareParameters(Scope scope, IEnumerable<IScopeAwareParameter> parameters)
    {
        _scope = scope;
        _parameters = parameters;
    }

    public ScopeAwareParameters(Scope scope, IParameters parameters) : 
        this(scope, parameters.GetParameters().Select(parameter => new ScopeAwareParameter(scope, parameter))) {}

    public IEnumerable<IScopeAwareParameter> GetParameters() => _parameters;

    public Scope Scope() => _scope;
}