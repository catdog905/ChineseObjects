using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

public interface ITypedReference : ITypedExpression
{
    public string Name();
}

public class TypedReference : ITypedReference
{
    private readonly string _name;
    private readonly Type _type;

    public TypedReference(string name, Type type)
    {
        _name = name;
        _type = type;
    }

    public TypedReference(IScopeAwareReference reference) :
        this(
            reference.Name(), 
            reference.Scope().GetValue(reference.Name()).Type()) {}


    public Type Type()
    {
        return _type;
    }

    public T AcceptVisitor<T>(CodeGen.ITypesAwareStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public string Name()
    {
        return _name;
    }

    public IList<string> GetRepr()
    {
        return new DeclarationAwareTree.Statement.Expression.Reference(this).GetRepr();
    }
}