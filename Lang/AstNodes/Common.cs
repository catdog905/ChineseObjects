using System.Collections.Immutable;

namespace ChineseObjects.Lang;

// An identifier. Note that it is used to express that an `Identifier`
// is an `Expression`. In more complex expressions that include identifiers
// (such as variable/method/class declaration, etc) identifier is stored
// as a mere `string` rather than the `Identifier` object.
public class Identifier : Expression {
    public readonly string Name;

    public Identifier(string name) {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}

public class Identifiers : IAstNode
{
    public readonly ImmutableList<string> Names;

    public Identifiers(IEnumerable<string> names)
    {
        Names = names.ToImmutableList();
    }

    public Identifiers(
        IEnumerable<Identifier> identifiers
    ) : this(identifiers.Select(i => i.Name)) {}

    // Have to declare the empty constructor separately to remove the ambiguity between
    // `Identifiers(params Identifier[])` and `Identifiers(params string[])`
    public Identifiers() : this (ImmutableList<string>.Empty) {}

    public Identifiers(params Identifier[] identifiers) : this(identifiers.ToList()) {}

    public Identifiers(params string[] names) : this(names.ToImmutableList()) {}

    public Identifiers(
        Identifiers identifiers,
        Identifier identifier
    ) : this(identifiers.Names.Add(identifier.Name)) {}

    public Identifiers(
        Identifier identifier,
        Identifiers identifiers
    ) : this(new[] {identifier.Name}.Concat(identifiers.Names)) {}

    public override string ToString() {
        return String.Join(",", Names);
    }
}
