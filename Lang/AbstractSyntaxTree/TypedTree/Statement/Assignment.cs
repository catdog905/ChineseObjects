using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement;

public interface ITypesAwareAssignment : ITypesAwareStatement
{
    public string Name();
    public ITypedExpression Expr();
}

public class TypesAwareAssignment : ITypesAwareAssignment
{
    private readonly string _name;
    private readonly ITypedExpression _expression;

    public TypesAwareAssignment(string name, ITypedExpression expression)
    {
        _name = name;
        _expression = expression;
    }

    public TypesAwareAssignment(IScopeAwareAssignment assignment) :
        this(
            assignment.VariableName().Value(),
            TypeIrrealizable.MakeTypedExpression(assignment.Expression())) {}


    public string Name()
    {
        return _name;
    }

    public ITypedExpression Expr()
    {
        return _expression;
    }

    public T AcceptVisitor<T>(CodeGen.ITypesAwareStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public IList<string> GetRepr()
    {
        return new DeclarationAwareTree.Statement.Assignment(this).GetRepr();
    }
}
