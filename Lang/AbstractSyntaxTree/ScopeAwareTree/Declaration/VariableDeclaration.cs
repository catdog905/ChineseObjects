namespace ChineseObjects.Lang;

public interface IScopeAwareVariableDeclaration : IVariableDeclaration, IScopeAwareAstNode
{}

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

    public ScopeAwareVariableDeclaration(Scope scope, IVariableDeclaration variableDeclaration) :
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