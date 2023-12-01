namespace ChineseObjects.Lang;

public interface ITypesAwareIfElse : ITypesAwareStatement
{
    public ITypedExpression Condition();
    public ITypesAwareStatementsBlock Then();
    public ITypesAwareStatementsBlock? Else();
}

public class TypesAwareIfElse : ITypesAwareIfElse
{
    private readonly ITypedExpression _condition;
    private readonly ITypesAwareStatementsBlock _then;
    private readonly ITypesAwareStatementsBlock? _else;

    public TypesAwareIfElse(
        ITypedExpression condition, 
        ITypesAwareStatementsBlock then, 
        ITypesAwareStatementsBlock? @else)
    {
        _condition = condition;
        _then = then;
        _else = @else;
    }

    public TypesAwareIfElse(IScopeAwareIfElse ifElse) :
        this(
            TypeIrrealizable.MakeTypedExpression(ifElse.Condition()),
            new TypesAwareStatementsBlock(ifElse.ThenBlock()),
            ifElse.ElseBlock() == null ? null : new TypesAwareStatementsBlock(ifElse.ElseBlock())) {}


    public ITypedExpression Condition()
    {
        return _condition;
    }

    public ITypesAwareStatementsBlock Then()
    {
        return _then;
    }

    public ITypesAwareStatementsBlock? Else()
    {
        return _else;
    }

    public T AcceptVisitor<T>(CodeGen.ITypesAwareStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}