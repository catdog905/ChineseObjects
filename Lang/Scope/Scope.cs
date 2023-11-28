using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public class Scope
{
    private readonly ImmutableDictionary<String, Type> _typeCollection;
    private readonly ImmutableDictionary<String, Entity> _valueCollection;

    public Scope(ImmutableDictionary<string, Type> typeCollection, ImmutableDictionary<string, Entity> valueCollection)
    {
        _typeCollection = typeCollection;
        _valueCollection = valueCollection;
    }

    public Scope(
        Scope scope,
        ImmutableDictionary<string, Type> typeCollection,
        ImmutableDictionary<string, Entity> valueCollection) :
        this(
            UpdateDict(scope._typeCollection, typeCollection), 
            UpdateDict(scope._valueCollection, valueCollection)) {}

    public Scope(
        Scope scope,
        Dictionary<string, Type> typeCollection) :
        this(
            UpdateDict(scope._typeCollection, typeCollection.ToImmutableDictionary()), 
            scope._valueCollection)
    {
    }

    public Scope(
        Scope scope,
        Dictionary<string, Entity> valueCollection) :
        this(
            scope._typeCollection, 
            UpdateDict(scope._valueCollection, valueCollection.ToImmutableDictionary()))
    {
    }

    public Scope(Dictionary<string, Type> typeCollection, Dictionary<string, Entity> valueCollection) :
        this(typeCollection.ToImmutableDictionary(), valueCollection.ToImmutableDictionary())
    {
    }

    public Scope(Dictionary<string, Type> typeCollection) :
        this(typeCollection, new Dictionary<string, Entity>())
    {
    }

    public Scope(Dictionary<string, Entity> valueCollection) :
        this(new Dictionary<string, Type>(), valueCollection)
    {
    }

    public Scope() : this(
        ImmutableDictionary<string, Type>.Empty,
        ImmutableDictionary<string, Entity>.Empty)
    {
    }

    public Scope(Scope scope, string refName, Type refType) :
        this(scope,
            new Dictionary<string, Entity>()
            {
                { refName, new Entity(refName, refType) }
            })
    {
    }

    public Type GetType(String typeName)
    {
        if (_typeCollection.TryGetValue(typeName, out Type value))
        {
            return value;
        }

        throw new NoSuchTypeInScope(typeName);
    }

    public Entity GetValue(String valueName)
    {
        if (_valueCollection.TryGetValue(valueName, out Entity value))
        {
            return value;
        }

        throw new NoSuchValueInScope(valueName);
    }


    public override string ToString()
    {
        return string.Join(Environment.NewLine, _typeCollection.Select(a => $"{a.Key}: {a.Value}"));
    }

    private static ImmutableDictionary<String, Type> UpdateDict(
        ImmutableDictionary<String, Type> dict1,
        ImmutableDictionary<String, Type> dict2)
    {
        return dict1.RemoveRange(dict2.Keys).AddRange(dict2);
    }

    private static ImmutableDictionary<String, Entity> UpdateDict(
        ImmutableDictionary<String, Entity> dict1,
        ImmutableDictionary<String, Entity> dict2)
    {
        return dict1.RemoveRange(dict2.Keys).AddRange(dict2);
    }
}

public class ScopeException : Exception
{
    protected ScopeException(string message) :
        base(message) {}
}

public class NoSuchValueInScope : ScopeException
{
    public NoSuchValueInScope(string valueName) : 
        base("Value with name " + valueName + " wasn't found in the scope") {}
}

public class NoSuchTypeInScope : ScopeException
{
    public NoSuchTypeInScope(string valueName) : 
        base("Type with name " + valueName + " wasn't found in the scope") {}
}