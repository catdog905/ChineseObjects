using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

public interface IReference : IExpression
{
    public string Name();
    
}

public class Reference : IReference
{
    private readonly string _name;

    public Reference(string name)
    {
        _name = name;
    }
    
    public Reference(IIdentifier identifier) :
        this(identifier.Value()) {}

    public Reference(ITypedReference reference) :
        this(reference.Name())
    {}


    public string Name()
    {
        return _name;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"REFERENCE " + _name};
    }
}