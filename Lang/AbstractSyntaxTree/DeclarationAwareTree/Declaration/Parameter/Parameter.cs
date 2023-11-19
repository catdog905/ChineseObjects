using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IParameterDeclaration : IParameter
{
    public IIdentifierDeclaration Name();
    public IIdentifierDeclaration TypeName();
}
    
// A parameter (is not an expression)
public class Parameter : IParameterDeclaration, IHumanReadable {
    private readonly IIdentifierDeclaration _name;
    private readonly IIdentifierDeclaration _typeName;

    public Parameter(IIdentifierDeclaration name, IIdentifierDeclaration identifier) {
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


    public IIdentifierDeclaration Name()
    {
        return _name;
    }

    public IIdentifierDeclaration TypeName()
    {
        return _typeName;
    }
}