using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public class Scope
{
    public readonly ImmutableDictionary<String, Type> TypeCollection;
    public readonly ImmutableDictionary<String, Value> ValueCollection;
    
    public Scope(ImmutableDictionary<string, Type> typeCollection, ImmutableDictionary<string, Value> valueCollection)
    {
        TypeCollection = typeCollection;
        ValueCollection = valueCollection;
    }
    
    public Scope(
        Scope scope,
        ImmutableDictionary<string, Type> typeCollection,
        ImmutableDictionary<string, Value> valueCollection) :
        this(scope.TypeCollection.AddRange(typeCollection), scope.ValueCollection.AddRange(valueCollection)) {}
    
    public Scope(
        Scope scope,
        Dictionary<string, Type> typeCollection) :
        this(scope.TypeCollection.AddRange(typeCollection), ImmutableDictionary<string, Value>.Empty) {}
    
    public Scope(
        Scope scope,
        Dictionary<string, Value> valueCollection) :
        this(ImmutableDictionary<string, Type>.Empty, scope.ValueCollection.AddRange(valueCollection)) {}
    
    public Scope(Dictionary<string, Type> typeCollection, Dictionary<string, Value> valueCollection) : 
        this(typeCollection.ToImmutableDictionary(), valueCollection.ToImmutableDictionary()) {}

    public Scope(Dictionary<string, Type> typeCollection) : 
        this(typeCollection, new Dictionary<string, Value>()) {}

    public Scope(Dictionary<string, Value> valueCollection) :
        this(new Dictionary<string, Type>(), valueCollection) {}

    public Type GetType(String typeName)
    {
        if (TypeCollection.TryGetValue(typeName, out Type value))
        {
            return value;
        }
        throw new NoSuchValueInScope();
    }

    public Value GetValue(String valueName)
    {
        if (ValueCollection.TryGetValue(valueName, out Value value))
        {
            return value;
        }
        throw new NoSuchValueInScope();
    }
}

public class ScopeException : Exception { }

public class NoSuchValueInScope : ScopeException { }