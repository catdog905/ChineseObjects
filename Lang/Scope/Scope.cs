using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public class Scope
{
    private readonly ImmutableDictionary<String, Type> _typeCollection;
    private readonly ImmutableDictionary<String, Value> _valueCollection;
    
    public Scope(ImmutableDictionary<string, Type> typeCollection, ImmutableDictionary<string, Value> valueCollection)
    {
        _typeCollection = typeCollection;
        _valueCollection = valueCollection;
    }
    
    public Scope(
        Scope scope,
        ImmutableDictionary<string, Type> typeCollection,
        ImmutableDictionary<string, Value> valueCollection) :
        this(scope._typeCollection.AddRange(typeCollection), scope._valueCollection.AddRange(valueCollection)) {}
    
    public Scope(
        Scope scope,
        Dictionary<string, Type> typeCollection) :
        this(scope._typeCollection.AddRange(typeCollection), ImmutableDictionary<string, Value>.Empty) {}
    
    public Scope(
        Scope scope,
        Dictionary<string, Value> valueCollection) :
        this(ImmutableDictionary<string, Type>.Empty, scope._valueCollection.AddRange(valueCollection)) {}
    
    public Scope(Dictionary<string, Type> typeCollection, Dictionary<string, Value> valueCollection) : 
        this(typeCollection.ToImmutableDictionary(), valueCollection.ToImmutableDictionary()) {}

    public Scope(Dictionary<string, Type> typeCollection) : 
        this(typeCollection, new Dictionary<string, Value>()) {}

    public Scope(Dictionary<string, Value> valueCollection) :
        this(new Dictionary<string, Type>(), valueCollection) {}
    
    public Scope() : this(
        ImmutableDictionary<string, Type>.Empty, 
        ImmutableDictionary<string, Value>.Empty) {}

    public Type GetType(String typeName)
    {
        if (_typeCollection.TryGetValue(typeName, out Type value))
        {
            return value;
        }
        throw new NoSuchValueInScope();
    }

    public Value GetValue(String valueName)
    {
        if (_valueCollection.TryGetValue(valueName, out Value value))
        {
            return value;
        }
        throw new NoSuchValueInScope();
    }
}

public class ScopeException : Exception { }

public class NoSuchValueInScope : ScopeException { }