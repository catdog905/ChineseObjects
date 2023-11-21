using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IParameterDeclaration
{
    public IDeclarationIdentifier Name();
    public IDeclarationIdentifier TypeName();
}
    
// A parameter (is not an expression)
public class Parameter : IParameterDeclaration, IHumanReadable {
    private readonly IDeclarationIdentifier _name;
    private readonly IDeclarationIdentifier _typeName;

    public Parameter(IDeclarationIdentifier name, IDeclarationIdentifier declarationIdentifier) {
        _name = name;
        _typeName = declarationIdentifier;
    }

    public override string ToString()
    {
        return _name + ": " + _typeName;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"PARAMETER " + _name + ": " + _typeName};
    }


    public IDeclarationIdentifier Name()
    {
        return _name;
    }

    public IDeclarationIdentifier TypeName()
    {
        return _typeName;
    }
}