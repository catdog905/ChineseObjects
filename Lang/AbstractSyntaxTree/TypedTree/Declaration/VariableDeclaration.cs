namespace ChineseObjects.Lang.Declaration;

public interface ITypedVariable : ITypedAstNode
{
    public IIdentifier Name();
}

public class TypedVariable : ITypedVariable
{
    private readonly IIdentifier _name;
    private readonly Type _type;

    public TypedVariable(IIdentifier name, Type type)
    {
        _name = name;
        _type = type;
    }
    
    public TypedVariable(IScopeAwareVariable variable) :
        this(
            variable.Name(), 
            new Type(variable.Scope(), variable.TypeName())) {}

    public IIdentifier Name()
    {
        return _name;
    }

    public Type Type()
    {
        return _type;
    }
}