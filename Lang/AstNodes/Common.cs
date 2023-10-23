namespace ChineseObjects.Lang;

// An identifier. Note that it is used to express that an `Identifier`
// is an `Expression`. In more complex expressions that include identifiers
// (such as variable/method/class declaration, etc) identifier is stored
// as a mere `string` rather than the `Identifier` object.
public class Identifier : Statement, Object{
    public readonly string name;

    public Identifier(string name) {
        this.name = name;
    }

    public override string ToString()
    {
        return name;
    }

    public List<IAstNode> Children()
    {
        return new List<IAstNode>();
    }

    public IAstNode CurrentNode()
    {
        return this;
    }
}

public class Identifiers : IAstNode
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

    public override string ToString()
    {
        return String.Join(",", Names);
    }

    public List<IAstNode> Children()
    {
        return IdentifiersList.Cast<IAstNode>().ToList();
    }

    public IAstNode CurrentNode()
    {
        return this;
    }
}