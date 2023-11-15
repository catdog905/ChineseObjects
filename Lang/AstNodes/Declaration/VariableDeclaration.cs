namespace ChineseObjects.Lang;

public interface IVariableDeclaration : MemberDeclaration
{
    public string Name();
    public string Type();
}

public interface IScopeAwareVariableDeclaration : MemberDeclaration, IScopeAwareAstNode
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

public class ScopeAwareVariableDeclaration : IScopeAwareVariableDeclaration
{
    private readonly Scope _scope;
    private readonly string _name;
    private readonly string _type;

    public ScopeAwareVariableDeclaration(Scope scope, string name, string type)
    {
        _scope = scope;
        _name = name;
        _type = type;
    }

    public ScopeAwareVariableDeclaration(Scope scope, VariableDeclaration variableDeclaration) :
        this(scope, variableDeclaration.Name(), variableDeclaration.Type()) {}

    public Scope Scope()
    {
        return _scope;
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