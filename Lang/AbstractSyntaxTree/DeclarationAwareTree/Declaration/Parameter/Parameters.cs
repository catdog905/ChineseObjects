using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IParameterDeclarations : IParameters, IDeclarationAstNode
{
    public IEnumerable<IParameterDeclaration> GetParameters();
}

// A list of parameters (is not an expression)
public class Parameters : IParameterDeclarations {
    private readonly ImmutableList<IParameterDeclaration> _parameters;

    public Parameters(IEnumerable<IParameterDeclaration> parames)
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

    IEnumerable<IParameter> IParameters.GetParameters()
    {
        return GetParameters();
    }

    public IEnumerable<IParameterDeclaration> GetParameters()
    {
        return _parameters;
    }
}