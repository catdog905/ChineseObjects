using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IParameters : IAstNode
{
    public IEnumerable<Parameter> GetParameters();
}

public interface IScopeAwareParameters : IParameters, IScopeAwareAstNode
{ 
    new IEnumerable<ScopeAwareParameter> GetParameters();
}

// A list of parameters (is not an expression)
public class Parameters : IParameters {
    private readonly ImmutableList<Parameter> _parameters;

    public Parameters(IEnumerable<Parameter> parames)
    {
        _parameters = parames.ToImmutableList();
    }

    public Parameters(
        Parameters parameters,
        Parameter parameter
    ) : this(parameters._parameters.Add(parameter)) {}

    public Parameters(
        Parameter parameter,
        Parameters parameters
    ) : this(new[] {parameter}.Concat(parameters._parameters)) {}

    public Parameters(params Parameter[] parameters) : this(parameters.ToList()) {}


    public override string ToString()
    {
        return String.Join(",", _parameters);
    }

    public IEnumerable<Parameter> GetParameters()
    {
        return _parameters;
    }
}

public class ScopeAwareParameters : IScopeAwareParameters
{
    private readonly Scope _scope;
    private readonly IEnumerable<ScopeAwareParameter> _parameters;

    public ScopeAwareParameters(Scope scope, IEnumerable<ScopeAwareParameter> parameters)
    {
        _scope = scope;
        _parameters = parameters;
    }

    public ScopeAwareParameters(Scope scope, Parameters parameters) : 
        this(scope, parameters.GetParameters().Select(parameter => new ScopeAwareParameter(scope, parameter))) {}


    IEnumerable<Parameter> IParameters.GetParameters()
    {
        throw new NotImplementedException();
    }

    public Scope Scope()
    {
        return _scope;
    }

    IEnumerable<ScopeAwareParameter> IScopeAwareParameters.GetParameters()
    {
        return _parameters;
    }
}