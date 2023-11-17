using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IParameters : IAstNode
{
    public IEnumerable<IParameter> GetParameters();
}

// A list of parameters (is not an expression)
public class Parameters : IParameters {
    private readonly ImmutableList<IParameter> _parameters;

    public Parameters(IEnumerable<IParameter> parames)
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

    public IEnumerable<IParameter> GetParameters()
    {
        return _parameters;
    }
}