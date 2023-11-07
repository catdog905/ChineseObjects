using System.Collections.Immutable;

namespace ChineseObjects.Lang;

// A parameter (is not an expression)
public class Parameter : IAstNode, IHumanReadable {
    public readonly string Name;
    public readonly string Type;

    public Parameter(string name, Identifier type) {
        Name = name;
        Type = type.Name;
    }

    public override string ToString()
    {
        return Name + ": " + Type;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"PARAMETER " + Name + ": " + Type};
    }
}

// A list of parameters (is not an expression)
public class Parameters : IAstNode {
    public readonly ImmutableList<Parameter> Parames;

    public Parameters(IEnumerable<Parameter> parames)
    {
        Parames = parames.ToImmutableList();
    }

    public Parameters(
        Parameters parameters,
        Parameter parameter
    ) : this(parameters.Parames.Add(parameter)) {}

    public Parameters(
        Parameter parameter,
        Parameters parameters
    ) : this(new[] {parameter}.Concat(parameters.Parames)) {}

    public Parameters(params Parameter[] parameters) : this(parameters.ToList()) {}


    public override string ToString()
    {
        return String.Join(",", Parames);
    }
}

public class ScopeAwareParameter
{
    
}