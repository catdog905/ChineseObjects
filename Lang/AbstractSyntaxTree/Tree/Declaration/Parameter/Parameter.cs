using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IParameter : IAstNode
{
    public string Name();
    public string TypeName();
}
    
// A parameter (is not an expression)
public class Parameter : IParameter, IHumanReadable {
    private readonly string _name;
    private readonly string _type;

    public Parameter(string name, Identifier type) {
        _name = name;
        _type = type.Name;
    }

    public override string ToString()
    {
        return _name + ": " + _type;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"PARAMETER " + _name + ": " + _type};
    }


    public string Name()
    {
        return _name;
    }

    public string TypeName()
    {
        return _type;
    }
}