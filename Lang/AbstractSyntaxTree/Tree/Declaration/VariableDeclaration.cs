namespace ChineseObjects.Lang;

public interface IVariableDeclaration : MemberDeclaration
{
    public string Name();
    public string Type();
}


// A variable declaration (is not an expression)
// TODO: should declarations of initialized and uninitialized variable
// be the same or different types of nodes?
public class VariableDeclaration : IVariableDeclaration, IHumanReadable
{
    private readonly string _name;
    private readonly string _type;

    public VariableDeclaration(string name, string typeName)
    {
        _name = name;
        _type = typeName;
    }
    
    public VariableDeclaration(Identifier name, Identifier type) : this(name.Name, type.Name) {}

    public override string ToString()
    {
        return _name + ":" + _type;
    }

    public IList<string> GetRepr()
    {
        return new List<string>{"VARIABLE " + _name + ": " + _type};
    }

    public string Name()
    {
        return _name;
    }

    public string Type()
    {
        return _type;
    }
}