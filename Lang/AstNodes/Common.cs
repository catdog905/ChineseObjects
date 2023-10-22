namespace ChineseObjects.Lang;

// An identifier. Note that it is used to express that an `Identifier`
// is an `Expression`. In more complex expressions that include identifiers
// (such as variable/method/class declaration, etc) identifier is stored
// as a mere `string` rather than the `Identifier` object.
public class Identifier {
    public readonly string name;

    public Identifier(string name) {
        this.name = name;
    }

    public override string ToString()
    {
        return name;
    }
}

public class Identifiers
{
    public readonly List<Identifier> IdentifiersList;
    public readonly List<string> Names = new List<string>();

    public Identifiers(List<Identifier> identifiersList)
    {
        IdentifiersList = identifiersList;
        foreach (Identifier identifier in identifiersList)
        {
            Names.Add(identifier.name);
        }
    }
    
    public Identifiers(params Identifier[] identifiers)
    {
        IdentifiersList = identifiers.ToList();
    }
    
    public Identifiers(Identifier identifier, Identifiers identifiers)
    {
        IdentifiersList = new List<Identifier> { identifier };
        IdentifiersList.AddRange(identifiers.IdentifiersList);
    }
}