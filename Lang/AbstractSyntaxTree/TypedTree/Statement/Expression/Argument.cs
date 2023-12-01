namespace ChineseObjects.Lang;

public interface ITypedArgument : ITypedAstNode
{
    public ITypedExpression Value();
}

public interface ITypesAwareArguments : ITypesAwareAstNode
{
    public IEnumerable<ITypedArgument> Values();
}

public class TypedArgument : ITypedArgument
{
    private readonly Type _type;
    private readonly ITypedExpression _value;

    public TypedArgument(Type type, ITypedExpression value)
    {
        _type = type;
        _value = value;
    }

    public TypedArgument(ITypedExpression value) :
        this(value.Type(), value) {}
    
    public TypedArgument(IScopeAwareArgument argument) :
        this(TypeIrrealizable.MakeTypedExpression(argument.Value())) {}

    public Type Type()
    {
        return _type;
    }

    public T AcceptVisitor<T>(CodeGen.ITypesAwareStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public ITypedExpression Value()
    {
        return _value;
    }
}

public class TypesAwareArguments : ITypesAwareArguments
{
    private readonly IEnumerable<ITypedArgument> _values;

    public TypesAwareArguments(IEnumerable<ITypedArgument> values)
    {
        _values = values;
    }

    public TypesAwareArguments(IScopeAwareArguments arguments) :
        this(arguments.Values()
            .Select(argument => new TypedArgument(argument))
            .ToList())
    {}


    public IEnumerable<ITypedArgument> Values()
    {
        return _values;
    }
}