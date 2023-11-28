namespace ChineseObjects.Lang;

public interface IScopeAwareReference : IScopeAwareExpression
{
    public string Name();
}

public class ScopeAwareReference : IScopeAwareReference
{
    private readonly Scope _scope;
    private readonly string _name;

    public ScopeAwareReference(Scope scope, string name)
    {
        _scope = scope;
        _name = name;
    }

    public ScopeAwareReference(Scope scope, Reference reference) :
        this(scope, reference.Name()) {}

    public Scope Scope()
    {
        return _scope;
    }

    public string Name()
    {
        return _name;
    }
}