namespace ChineseObjects.Lang.Expression;

public interface ITypedIdentifier : IDeclarationIdentifier, ITypedAstNode { }

public class TypedAwareIdentifier : ITypedIdentifier
{
    private readonly Type _type;
    private readonly string _name;

    public TypedAwareIdentifier(Type type, string name)
    {
        _type = type;
        _name = name;
    }

    public TypedAwareIdentifier(IScopeAwareDeclarationIdentifier declarationIdentifier) :
        this(declarationIdentifier.Scope().GetType(declarationIdentifier.Name()), declarationIdentifier.Name()) {}


    public string Name() => _name;

    public Type Type() => _type;
    public IList<string> GetRepr()
    {
        throw new NotImplementedException();
    }
}
