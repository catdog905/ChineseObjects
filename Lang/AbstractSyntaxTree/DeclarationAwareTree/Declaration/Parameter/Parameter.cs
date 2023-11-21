using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IParameterDeclaration
{
    public IIdentifier Name();
    public IIdentifier TypeName();
}
    
// A parameter (is not an expression)
public class Parameter : IParameterDeclaration, IHumanReadable {
    private readonly IIdentifier _name;
    private readonly IIdentifier _typeName;

    public Parameter(IIdentifier name, IIdentifier identifier) {
        _name = name;
        _typeName = identifier;
    }

    public override string ToString()
    {
        return _name + ": " + _typeName;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"PARAMETER " + _name + ": " + _typeName};
    }


    public IIdentifier Name()
    {
        return _name;
    }

    public IIdentifier TypeName()
    {
        return _typeName;
    }
}