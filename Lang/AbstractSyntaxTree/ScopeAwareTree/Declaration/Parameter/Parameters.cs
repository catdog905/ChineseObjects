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

    public ScopeAwareParameters(Scope scope, IParameters parameters) : 
        this(scope, parameters.GetParameters().Select(parameter => new ScopeAwareParameter(scope, parameter))) {}
    

    public Scope Scope()
    {
        return _scope;
    }

    public IEnumerable<IScopeAwareParameter> GetParameters()
    {
        return _parameters;
    }

    IEnumerable<IParameter> IParameters.GetParameters()
    {
        return GetParameters();
    }
}