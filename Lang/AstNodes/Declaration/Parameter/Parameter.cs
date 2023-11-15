using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IParameter : IAstNode
{
    public string Name();
    public string Type();
}

public interface IScopeAwareParameter : IParameter, IScopeAwareAstNode
{
    new string Name();
    new string Type();
}
    
// A parameter (is not an expression)
public class Parameter : IParameter, IHumanReadable {
    private readonly string _name;
    private readonly string _type;

    public Parameter(string name, Identifier type) {
        _name = name;
        _type = type.Name;
    }

    public override string ToString()
    {
        return _name + ": " + _type;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"PARAMETER " + _name + ": " + _type};
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
        this(scope, parameter.Name(), parameter.Type()) {}

    string IParameter.Name()
    {
        throw new NotImplementedException();
    }

    string IScopeAwareParameter.Type()
    {
        return _type;
    }

    string IScopeAwareParameter.Name()
    {
        return _name;
    }

    string IParameter.Type()
    {
        throw new NotImplementedException();
    }

    public Scope Scope()
    {
        return _scope;
    }
}