namespace ChineseObjects.Lang.Declaration;

public interface ITypesAwareVariableDeclaration : IVariableDeclaration, ITypesAwareAstNode
{
    public Type Type();
}

public class TypesAwareVariableDeclaration : ITypesAwareVariableDeclaration
{
    private readonly string _name;
    private readonly string _typeName;
    private readonly Type _type;

    public TypesAwareVariableDeclaration(string name, string typeName, Type type)
    {
        _name = name;
        _typeName = typeName;
        _type = type;
    }
    
    public TypesAwareVariableDeclaration(IScopeAwareVariableDeclaration variableDeclaration) :
        this(
            variableDeclaration.Name(), 
            variableDeclaration.TypeName(), 
            new Type(variableDeclaration.Scope(), variableDeclaration.TypeName())) {}

    public string Name()
    {
        return _name;
    }

    public string TypeName()
    {
        return _typeName;
    }

    public Type Type()
    {
        return _type;
    }
}