namespace ChineseObjects.Lang;

public interface IVariableDeclaration : IMemberDeclaration, IDeclarationAstNode
{
    public IDeclarationIdentifier Name();
    public IDeclarationIdentifier TypeName();
}


// A variable declaration (is not an expression)
// TODO: should declarations of initialized and uninitialized variable
// be the same or different types of nodes?
public class VariableDeclaration : IVariableDeclaration, IHumanReadable
{
    private readonly IDeclarationIdentifier _name;
    private readonly IDeclarationIdentifier _typeName;

    public VariableDeclaration(IDeclarationIdentifier name, IDeclarationIdentifier typeNameName)
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

    public IDeclarationIdentifier Name()
    {
        return _name;
    }

    public IDeclarationIdentifier TypeName()
    {
        return _typeName;
    }
}