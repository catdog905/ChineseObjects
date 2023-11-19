using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IIdentifierDeclaration : IIdentifier, IExpressionDeclaration {}

// An identifier. Note that it is used to express that an `Identifier`
// is an `Expression`. In more complex expressions that include identifiers
// (such as variable/method/class declaration, etc) identifier is stored
// as a mere `string` rather than the `Identifier` object.
public class Identifier : IIdentifierDeclaration {
    private readonly string _name;

    public Identifier(string name) {
        _name = name;
    }

    public override string ToString()
    {
        return _name;
    }

    public string Name()
    {
        return _name;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"IDENTIFIER " + _name};
    }
}

public interface IDeclarationIdentifiers : IIdentifiers, IDeclarationAstNode
{
    public new IEnumerable<IIdentifierDeclaration> GetIdentifiers();
}

public class Identifiers : IDeclarationIdentifiers
{
    private readonly ImmutableList<IIdentifierDeclaration> _names;

    public Identifiers(IEnumerable<IIdentifierDeclaration> names)
    {
        _names = names.ToImmutableList();
    }
    // Have to declare the empty constructor separately to remove the ambiguity between
    // `Identifiers(params Identifier[])` and `Identifiers(params string[])`
    public Identifiers() : this (ImmutableList<IIdentifierDeclaration>.Empty) {}

    public Identifiers(params IIdentifierDeclaration[] identifiers) : this(identifiers.ToList()) {}

    public Identifiers(params string[] names) : this(names.Select(name => new Identifier(name))) {}

    public Identifiers(
        Identifiers identifiers,
        IIdentifierDeclaration identifier
    ) : this(identifiers._names.Add(identifier)) {}

    public Identifiers(
        IIdentifierDeclaration identifier,
        Identifiers identifiers
    ) : this(new[] {identifier}.Concat(identifiers._names)) {}

    public override string ToString() {
        return String.Join(",", _names);
    }

    IEnumerable<IIdentifier> IIdentifiers.GetIdentifiers()
    {
        throw new NotImplementedException();
    }

    IEnumerable<IIdentifierDeclaration> IDeclarationIdentifiers.GetIdentifiers()
    {
        return _names;
    }
}
