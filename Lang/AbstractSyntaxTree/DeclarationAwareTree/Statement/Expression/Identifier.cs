using System.Collections.Immutable;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

public interface IIdentifier
{
    public string Value();
}

// An identifier. Note that it is used to express that an `Identifier`
// is an `Expression`. In more complex expressions that include identifiers
// (such as variable/method/class declaration, etc) identifier is stored
// as a mere `string` rather than the `Identifier` object.
public class Identifier : IIdentifier {
    private readonly string _name;

    public Identifier(string name) {
        _name = name;
    }

    public override string ToString()
    {
        return _name;
    }

    public string Value()
    {
        return _name;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"IDENTIFIER " + _name};
    }
}

public interface IIdentifiers
{
    public IEnumerable<IIdentifier> GetIdentifiers();
}

public class Identifiers : IIdentifiers
{
    private readonly ImmutableList<IIdentifier> _names;

    public Identifiers(IEnumerable<IIdentifier> names)
    {
        _names = names.ToImmutableList();
    }
    // Have to declare the empty constructor separately to remove the ambiguity between
    // `Identifiers(params Identifier[])` and `Identifiers(params string[])`
    public Identifiers() : this (ImmutableList<IIdentifier>.Empty) {}

    public Identifiers(params IIdentifier[] identifiers) : this(identifiers.ToList()) {}

    public Identifiers(params string[] names) : this(names.Select(name => new Identifier(name))) {}

    public Identifiers(
        Identifiers identifiers,
        IIdentifier identifier
    ) : this(identifiers._names.Add(identifier)) {}

    public Identifiers(
        IIdentifier identifier,
        Identifiers identifiers
    ) : this(new[] {identifier}.Concat(identifiers._names)) {}

    public override string ToString() {
        return String.Join(",", _names);
    }

    IEnumerable<IIdentifier> IIdentifiers.GetIdentifiers()
    {
        return _names;
    }
}
