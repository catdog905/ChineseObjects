using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareParameter : IParameter, IScopeAwareAstNode
{
    public IScopeAwareIdentifier Name();
    public IScopeAwareIdentifier TypeName();
}

public class ScopeAwareParameter : IScopeAwareParameter
{
    private readonly Scope _scope;
    private readonly IScopeAwareIdentifier _name;
    private readonly IScopeAwareIdentifier _typeName;

    public ScopeAwareParameter(Scope scope, IScopeAwareIdentifier name, IScopeAwareIdentifier typeName)
    {
        _scope = scope;
        _name = name;
        _typeName = typeName;
    }
    
    public ScopeAwareParameter(Scope scope, IParameterDeclaration parameterDeclaration) : 
        this(scope, 
            new ScopeAwareIdentifier(scope,parameterDeclaration.Name()), 
            new ScopeAwareIdentifier(scope, parameterDeclaration.TypeName())) {}

    public Scope Scope() => _scope;
    public IScopeAwareIdentifier TypeName() => _typeName;
    public IScopeAwareIdentifier Name() => _name;
}