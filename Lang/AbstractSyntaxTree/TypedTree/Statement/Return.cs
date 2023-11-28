namespace ChineseObjects.Lang;

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
}