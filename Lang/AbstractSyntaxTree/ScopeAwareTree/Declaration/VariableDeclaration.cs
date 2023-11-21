namespace ChineseObjects.Lang;

public interface IScopeAwareVariable : IScopeAwareAstNode
{
    public IScopeAwareIdentifier Name();
    public IScopeAwareIdentifier TypeName();
}

public class ScopeAwareVariable : IScopeAwareVariable
{
    private readonly Scope _scope;
    private readonly IScopeAwareIdentifier _name;
    private readonly IScopeAwareIdentifier _type;

    public ScopeAwareVariable(Scope scope, IScopeAwareIdentifier name, IScopeAwareIdentifier type)
    {
        _scope = scope;
        _name = name;
        _type = type;
    }

    public ScopeAwareVariable(Scope scope, IVariableDeclaration variableDeclaration) :
        this(
            scope, 
            new ScopeAwareIdentifier(scope, variableDeclaration.Identifier()), 
            new ScopeAwareIdentifier(scope , variableDeclaration.TypeName())) {}

    public Scope Scope()
    {
        return _scope;
    }

    public IScopeAwareIdentifier Name()
    {
        return _name;
    }

    public IScopeAwareIdentifier TypeName()
    {
        return _type;
    }
}