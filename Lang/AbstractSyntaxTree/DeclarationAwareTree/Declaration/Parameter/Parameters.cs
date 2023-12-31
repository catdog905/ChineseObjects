using System.Collections.Immutable;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration.Parameter;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration.Parameter;

public interface IParameters
{
    public IEnumerable<IParameterDeclaration> GetParameters();
}

// A list of parameters (is not an expression)
public class Parameters : IParameters {
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

    public Parameters(ITypesAwareParameters parameters) :
        this(parameters.GetParameters()
            .Select(parameter => new Parameter(parameter)))
    {}


    public override string ToString()
    {
        return String.Join(",", _parameters);
    }

    public IEnumerable<IParameterDeclaration> GetParameters()
    {
        return _parameters;
    }
}