namespace ChineseObjects.Lang.Expression;

public interface ITypedIdentifier : IIdentifier, ITypedAstNode { }

public class TypedAwareIdentifier : ITypedIdentifier
{
    private readonly Type _type;
    private readonly string _name;

    public TypedAwareIdentifier(Type type, string name)
    {
        _type = type;
        _name = name;
    }

    public TypedAwareIdentifier(IScopeAwareIdentifier identifier) :
        this(identifier.Scope().GetType(identifier.Name()), identifier.Name()) {}


    public string Name() => _name;

    public Type Type() => _type;
}
