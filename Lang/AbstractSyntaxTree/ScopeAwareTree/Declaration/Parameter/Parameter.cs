using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareParameter : IScopeAwareAstNode
{
    public IScopeAwareDeclarationIdentifier Name();
    public IScopeAwareDeclarationIdentifier TypeName();
}

public class ScopeAwareParameter : IScopeAwareParameter
{
    private readonly Scope _scope;
    private readonly IScopeAwareDeclarationIdentifier _name;
    private readonly IScopeAwareDeclarationIdentifier _typeName;

    public ScopeAwareParameter(Scope scope, IScopeAwareDeclarationIdentifier name, IScopeAwareDeclarationIdentifier typeName)
    {
        _scope = scope;
        _name = name;
        _typeName = typeName;
    }
    
    public ScopeAwareParameter(Scope scope, IParameterDeclaration parameterDeclaration) : 
        this(scope, 
            new ScopeAwareDeclarationIdentifier(scope,parameterDeclaration.Name()), 
            new ScopeAwareDeclarationIdentifier(scope, parameterDeclaration.TypeName())) {}

    public Scope Scope() => _scope;
    public IScopeAwareDeclarationIdentifier TypeName() => _typeName;
    public IScopeAwareDeclarationIdentifier Name() => _name;
}