using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IDeclarationIdentifier : IExpressionDeclaration
{
    public string Name();
}

// An identifier. Note that it is used to express that an `Identifier`
// is an `Expression`. In more complex expressions that include identifiers
// (such as variable/method/class declaration, etc) identifier is stored
// as a mere `string` rather than the `Identifier` object.
public class DeclarationIdentifier : IDeclarationIdentifier {
    private readonly string _name;

    public DeclarationIdentifier(string name) {
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

public interface IDeclarationIdentifiers : IDeclarationAstNode
{
    public IEnumerable<IDeclarationIdentifier> GetIdentifiers();
}

public class Identifiers : IDeclarationIdentifiers
{
    private readonly ImmutableList<IDeclarationIdentifier> _names;

    public Identifiers(IEnumerable<IDeclarationIdentifier> names)
    {
        _names = names.ToImmutableList();
    }
    // Have to declare the empty constructor separately to remove the ambiguity between
    // `Identifiers(params Identifier[])` and `Identifiers(params string[])`
    public Identifiers() : this (ImmutableList<IDeclarationIdentifier>.Empty) {}

    public Identifiers(params IDeclarationIdentifier[] identifiers) : this(identifiers.ToList()) {}

    public Identifiers(params string[] names) : this(names.Select(name => new DeclarationIdentifier(name))) {}

    public Identifiers(
        Identifiers identifiers,
        IDeclarationIdentifier declarationIdentifier
    ) : this(identifiers._names.Add(declarationIdentifier)) {}

    public Identifiers(
        IDeclarationIdentifier declarationIdentifier,
        Identifiers identifiers
    ) : this(new[] {declarationIdentifier}.Concat(identifiers._names)) {}

    public override string ToString() {
        return String.Join(",", _names);
    }

    IEnumerable<IDeclarationIdentifier> IDeclarationIdentifiers.GetIdentifiers()
    {
        return _names;
    }
}
