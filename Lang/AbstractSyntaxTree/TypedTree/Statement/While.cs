namespace ChineseObjects.Lang;

public interface ITypesAwareWhile : ITypesAwareStatement
{
    public ITypedExpression Condition();
    public ITypesAwareStatementsBlock Body();
}

public class TypesAwareWhile : ITypesAwareWhile
{
    private readonly ITypedExpression _condition;
    private readonly ITypesAwareStatementsBlock _body;

    public TypesAwareWhile(ITypedExpression condition, ITypesAwareStatementsBlock body)
    {
        _condition = condition;
        _body = body;
    }

    public TypesAwareWhile(IScopeAwareWhile scopeAwareWhile) :
        this(
            TypeIrrealizable.MakeTypedExpression(scopeAwareWhile.Condition()),
            new TypesAwareStatementsBlock(scopeAwareWhile.Body())) {}


    public ITypedExpression Condition()
    {
        return _condition;
    }

    public ITypesAwareStatementsBlock Body()
    {
        return _body;
    }

    public T AcceptVisitor<T>(CodeGen.ITypesAwareStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}