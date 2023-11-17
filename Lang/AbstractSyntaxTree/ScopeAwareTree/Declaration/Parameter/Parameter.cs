using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareParameter : IParameter, IScopeAwareAstNode
{}

public class ScopeAwareParameter : IScopeAwareParameter
{
    private readonly Scope _scope;
    private readonly string _name;
    private readonly string _type;

    public ScopeAwareParameter(Scope scope, string name, string type)
    {
        _scope = scope;
        _name = name;
        _type = type;
    }
    
    public ScopeAwareParameter(Scope scope, IParameter parameter) : 
        this(scope, parameter.Name(), parameter.TypeName()) {}

    public Scope Scope()
    {
        return _scope;
    }
    
    public string TypeName() => _type;
    public string Name() => _name;
}