using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public class Argument
{
    public readonly Expression Value;

    public Argument(Expression value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value?.ToString() ?? "NULL";
    }
}

public class Arguments
{
    public readonly ImmutableList<Argument> Values;

    public Arguments(IEnumerable<Argument> values)
    {
        Values = values.ToImmutableList();
    }

    public Arguments(params Argument[] arguments) : this(arguments.ToImmutableList()) { }

    public Arguments(Arguments arguments, Argument argument) : this(arguments.Values.Add(argument)) { }

    public Arguments(Argument argument, Arguments arguments) : this(new[] { argument }.Concat(arguments.Values)) { }

    public override string ToString()
    {
        return String.Join(",", Values);
    }
}