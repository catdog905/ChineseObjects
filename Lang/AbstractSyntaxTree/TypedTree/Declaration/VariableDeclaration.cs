namespace ChineseObjects.Lang.Declaration;

public interface ITypedVariable : ITypedAstNode
{
    public IDeclarationIdentifier Name();
}

public class TypedVariable : ITypedVariable
{
    private readonly IDeclarationIdentifier _name;
    private readonly Type _type;

    public TypedVariable(IDeclarationIdentifier name, Type type)
    {
        _name = name;
        _type = type;
    }
    
    public TypedVariable(IScopeAwareVariable variable) :
        this(
            variable.Name(), 
            new Type(variable.Scope(), variable.TypeName())) {}

    public IDeclarationIdentifier Name()
    {
        return _name;
    }

    public Type Type()
    {
        return _type;
    }
}