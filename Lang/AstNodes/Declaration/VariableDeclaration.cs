namespace ChineseObjects.Lang;

// A variable declaration (is not an expression)
// TODO: should declarations of initialized and uninitialized variable
// be the same or different types of nodes?
public class VariableDeclaration : MemberDeclaration, IHumanReadable
{
    public readonly string Name;
    public readonly string Type;

    public VariableDeclaration(string name, string typeName)
    {
        Name = name;
        Type = typeName;
    }
    
    public VariableDeclaration(Identifier name, Identifier type) : this(name.Name, type.Name) {}

    public override string ToString()
    {
        return Name + ":" + Type;
    }

    public IList<string> GetRepr()
    {
        return new List<string>{"VARIABLE " + Name + ": " + Type};
    }
}

public class ScopeAwareVariableDeclaration : VariableDeclaration
{
    public ScopeAwareVariableDeclaration(Scope scope, VariableDeclaration variableDeclaration) : 
        base(variableDeclaration.Name, variableDeclaration.Type)
    {
        throw new NotImplementedException();
    }
}