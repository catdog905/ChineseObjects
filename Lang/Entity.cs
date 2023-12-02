using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

namespace ChineseObjects.Lang;

public class Entity
{
    private readonly string _name;
    private readonly Type _type;

    public Entity(string name, Type type)
    {
        _name = name;
        _type = type;
    }

    public Entity(IIdentifier identifier, Type type) :
        this(identifier.Value(), type) { }

    public string Name() => _name;

    public Type Type() => _type;
}