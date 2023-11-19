namespace ChineseObjects.Lang;

public interface IVariableDeclaration : IVariable, IMemberDeclaration, IDeclarationAstNode
{
    public IIdentifierDeclaration Name();
    public IIdentifierDeclaration TypeName();
}


// A variable declaration (is not an expression)
// TODO: should declarations of initialized and uninitialized variable
// be the same or different types of nodes?
public class VariableDeclaration : IVariableDeclaration, IHumanReadable
{
    private readonly IIdentifierDeclaration _name;
    private readonly IIdentifierDeclaration _typeName;

    public VariableDeclaration(IIdentifierDeclaration name, IIdentifierDeclaration typeNameName)
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

    public IIdentifierDeclaration Name()
    {
        return _name;
    }

    public IIdentifierDeclaration TypeName()
    {
        return _typeName;
    }
}