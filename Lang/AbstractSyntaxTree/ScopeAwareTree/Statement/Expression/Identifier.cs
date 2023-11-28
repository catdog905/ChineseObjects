using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareIdentifier
{
    public string Value();
}

// An identifier. Note that it is used to express that an `Identifier`
// is an `Expression`. In more complex expressions that include identifiers
// (such as variable/method/class declaration, etc) identifier is stored
// as a mere `string` rather than the `Identifier` object.
public class ScopeAwareIdentifier : IScopeAwareIdentifier
{
    private readonly Scope _scope;
    private readonly string _name;

    public ScopeAwareIdentifier(Scope scope, string name)
    {
        _scope = scope;
        _name = name;
    }
    
    public ScopeAwareIdentifier(Scope scope, IIdentifier identifier) :
        this(scope, identifier.Value()) {}

    public override string ToString()
    {
        return _name;
    }

    public Scope Scope()
    {
        return _scope;
    }

    public string Value()
    {
        return _name;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"IDENTIFIER " + _name};
    }
}
