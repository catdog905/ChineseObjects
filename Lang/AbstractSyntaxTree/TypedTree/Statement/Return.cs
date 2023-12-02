using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement;

public interface ITypesAwareReturn : ITypesAwareStatement
{
    public ITypedExpression Expression();
}

public class TypesAwareReturn : ITypesAwareReturn
{
    private readonly ITypedExpression _expression;

    public TypesAwareReturn(ITypedExpression expression)
    {
        _expression = expression;
    }

    public TypesAwareReturn(IScopeAwareReturn scopeAwareReturn) :
        this(TypeIrrealizable.MakeTypedExpression(scopeAwareReturn.ReturnValue()))
    {}

    public ITypedExpression Expression()
    {
        return _expression;
    }

    public IList<string> GetRepr()
    {
        return new DeclarationAwareTree.Statement.Return(this).GetRepr();
    }
}