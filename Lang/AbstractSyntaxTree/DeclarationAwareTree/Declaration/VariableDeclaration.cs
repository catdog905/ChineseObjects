using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration;

public interface IVariableDeclaration : IMemberDeclaration, IAstNode
{
    public IIdentifier Identifier();
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

    public VariableDeclaration(ITypedVariable decl) :
        this(new Identifier(decl.Name()), decl.Type().TypeName()) {}

    public override string ToString()
    {
        return _name + ":" + _typeName;
    }

    public IList<string> GetRepr()
    {
        return new List<string>{"VARIABLE " + _name + ": " + _typeName};
    }

    public IIdentifier Identifier()
    {
        return _name;
    }

    public IIdentifier TypeName()
    {
        return _typeName;
    }
}