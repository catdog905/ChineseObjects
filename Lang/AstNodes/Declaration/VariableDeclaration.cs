namespace ChineseObjects.Lang;

// A variable declaration (is not an expression)
// TODO: should declarations of initialized and uninitialized variable
// be the same or different types of nodes?
public class VariableDeclaration : MemberDeclaration, IHumanReadable
{
    public readonly string Name;
    public readonly string Type;

    public VariableDeclaration(Identifier name, Identifier type)
    {
        Name = name.Name;
        Type = type.Name;
    }

    public override string ToString()
    {
        return Name + ":" + Type;
    }

    public IList<string> GetRepr()
    {
        return new List<string>{"VARIABLE " + Name + ": " + Type};
    }
}