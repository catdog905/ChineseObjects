using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IArgument : IExpression
{
    public IExpression Value();
}

public interface IArguments : IAstNode
{
    public IEnumerable<IArgument> Values();
}

public class Argument : IArgument
{
    private readonly IExpression _value;

    public Argument(IExpression value)
    {
        _value = value;
    }

    public override string ToString()
    {
        return _value?.ToString() ?? "NULL";
    }

    public IList<string> GetRepr()
    {
        return new List<string> { ToString() };
    }

    public IExpression Value()
    {
        return _value;
    }
}

public class Arguments : IArguments
{
    private readonly ImmutableList<IArgument> _values;

    public Arguments(IEnumerable<IArgument> values)
    {
        _values = values.ToImmutableList();
    }

    public Arguments(params IArgument[] arguments) : this(arguments.ToImmutableList()) { }

    public Arguments(Arguments arguments, IArgument argument) : this(arguments._values.Add(argument)) { }

    public Arguments(IArgument argument, Arguments arguments) : this(new[] { argument }.Concat(arguments._values)) { }

    public override string ToString()
    {
        return String.Join(",", _values);
    }

    public IEnumerable<IArgument> Values()
    {
        return _values;
    }
}