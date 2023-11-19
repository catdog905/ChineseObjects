using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IArgumentDeclaration : IArgument, IExpressionDeclaration
{
    public IExpressionDeclaration Value();
}

public interface IDeclarationArguments : IArguments, IDeclarationAstNode
{
    public IEnumerable<IArgumentDeclaration> Values();
}

public class Argument : IArgumentDeclaration
{
    private readonly IExpressionDeclaration _value;

    public Argument(IExpressionDeclaration value)
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

    public IExpressionDeclaration Value()
    {
        return _value;
    }
}

public class Arguments : IDeclarationArguments
{
    private readonly ImmutableList<IArgumentDeclaration> _values;

    public Arguments(IEnumerable<IArgumentDeclaration> values)
    {
        _values = values.ToImmutableList();
    }

    public Arguments(params IArgumentDeclaration[] arguments) : this(arguments.ToImmutableList()) { }

    public Arguments(Arguments arguments, IArgumentDeclaration argument) : this(arguments._values.Add(argument)) { }

    public Arguments(IArgumentDeclaration argument, Arguments arguments) : this(new[] { argument }.Concat(arguments._values)) { }

    public override string ToString()
    {
        return String.Join(",", _values);
    }

    public IEnumerable<IArgumentDeclaration> Values()
    {
        return _values;
    }
}