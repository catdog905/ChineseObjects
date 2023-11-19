namespace ChineseObjects.Lang.Declaration;

public interface ITypesAwareVariable : IVariable, ITypesAwareAstNode
{
    public IIdentifier Name();
    public Type Type();
}

public class TypesAwareVariable : ITypesAwareVariable
{
    private readonly IIdentifier _name;
    private readonly Type _type;

    public TypesAwareVariable(IIdentifier name, Type type)
    {
        _name = name;
        _type = type;
    }
    
    public TypesAwareVariable(IScopeAwareVariable variable) :
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