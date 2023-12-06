using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Declaration;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;

public interface ITypedVariable : ITypedAstNode
{
    public string Name();
}

public class TypedVariable : ITypedVariable
{
    private readonly string _name;
    private readonly Type _type;

    public TypedVariable(string name, Type type)
    {
        _name = name;
        _type = type;
    }
    
    public TypedVariable(IScopeAwareVariable variable) :
        this(
            variable.Name().Value(), 
            new Type(variable.Scope(), variable.TypeName())) {}

    public string Name()
    {
        return _name;
    }

    public Type Type()
    {
        return _type;
    }

    public IList<string> GetRepr()
    {
        return new DeclarationAwareTree.Declaration.VariableDeclaration(this).GetRepr();
    }
}