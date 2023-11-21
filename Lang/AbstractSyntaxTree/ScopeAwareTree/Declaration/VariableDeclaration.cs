namespace ChineseObjects.Lang;

public interface IScopeAwareVariable : IScopeAwareAstNode
{
    public IScopeAwareDeclarationIdentifier Name();
    public IScopeAwareDeclarationIdentifier TypeName();
}

public class ScopeAwareVariable : IScopeAwareVariable
{
    private readonly Scope _scope;
    private readonly IScopeAwareDeclarationIdentifier _name;
    private readonly IScopeAwareDeclarationIdentifier _type;

    public ScopeAwareVariable(Scope scope, IScopeAwareDeclarationIdentifier name, IScopeAwareDeclarationIdentifier type)
    {
        _scope = scope;
        _name = name;
        _type = type;
    }

    public ScopeAwareVariable(Scope scope, IVariableDeclaration variableDeclaration) :
        this(
            scope, 
            new ScopeAwareDeclarationIdentifier(scope, variableDeclaration.Name()), 
            new ScopeAwareDeclarationIdentifier(scope , variableDeclaration.TypeName())) {}

    public Scope Scope()
    {
        return _scope;
    }

    public IScopeAwareDeclarationIdentifier Name()
    {
        return _name;
    }

    public IScopeAwareDeclarationIdentifier TypeName()
    {
        return _type;
    }
}