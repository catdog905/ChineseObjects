namespace ChineseObjects.Lang;

public interface IVariableDeclaration : IMemberDeclaration, IAstNode
{
    public IIdentifier Name();
    public IIdentifier TypeName();
}


// A variable declaration (is not an expression)
// TODO: should declarations of initialized and uninitialized variable
// be the same or different types of nodes?
public class VariableDeclaration : IVariableDeclaration, IHumanReadable
{
    private readonly IIdentifier _name;
    private readonly IIdentifier _typeName;

    public VariableDeclaration(IIdentifier name, IIdentifier typeNameName)
    {
        _name = name;
        _typeName = typeNameName;
    }

    public override string ToString()
    {
        return _name + ":" + _typeName;
    }

    public IList<string> GetRepr()
    {
        return new List<string>{"VARIABLE " + _name + ": " + _typeName};
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