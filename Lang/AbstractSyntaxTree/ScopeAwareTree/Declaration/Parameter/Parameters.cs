using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareParameters : IParameters, IScopeAwareAstNode
{
    public new IEnumerable<IScopeAwareParameter> GetParameters();
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

    public ScopeAwareParameters(Scope scope, IParameterDeclarations parameterDeclarations) : 
        this(scope, parameterDeclarations.GetParameters().Select(parameter => new ScopeAwareParameter(scope, parameter))) {}

    public IEnumerable<IScopeAwareParameter> GetParameters() => _parameters;

    public Scope Scope() => _scope;
    IEnumerable<IParameter> IParameters.GetParameters()
    {
        return GetParameters();
    }
}