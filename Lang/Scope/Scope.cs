using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public class Scope
{
    private readonly ImmutableDictionary<String, Type> _typeCollection;
    private readonly ImmutableDictionary<String, Reference> _valueCollection;
    
    public Scope(ImmutableDictionary<string, Type> typeCollection, ImmutableDictionary<string, Reference> valueCollection)
    {
        _typeCollection = typeCollection;
        _valueCollection = valueCollection;
    }
    
    public Scope(
        Scope scope,
        ImmutableDictionary<string, Type> typeCollection,
        ImmutableDictionary<string, Reference> valueCollection) :
        this(scope._typeCollection.AddRange(typeCollection), scope._valueCollection.AddRange(valueCollection)) {}
    
    public Scope(
        Scope scope,
        Dictionary<string, Type> typeCollection) :
        this(scope._typeCollection.AddRange(typeCollection), scope._valueCollection) {}
    
    public Scope(
        Scope scope,
        Dictionary<string, Reference> valueCollection) :
        this(scope._typeCollection, scope._valueCollection.AddRange(valueCollection)) {}
    
    public Scope(Dictionary<string, Type> typeCollection, Dictionary<string, Reference> valueCollection) : 
        this(typeCollection.ToImmutableDictionary(), valueCollection.ToImmutableDictionary()) {}

    public Scope(Dictionary<string, Type> typeCollection) : 
        this(typeCollection, new Dictionary<string, Reference>()) {}

    public Scope(Dictionary<string, Reference> valueCollection) :
        this(new Dictionary<string, Type>(), valueCollection) {}
    
    public Scope() : this(
        ImmutableDictionary<string, Type>.Empty, 
        ImmutableDictionary<string, Reference>.Empty) {}

    public Type GetType(String typeName)
    {
        if (_typeCollection.TryGetValue(typeName, out Type value))
        {
            return value;
        }
        throw new NoSuchValueInScope();
    }

    public Reference GetValue(String valueName)
    {
        if (_valueCollection.TryGetValue(valueName, out Reference value))
        {
            return value;
        }
        throw new NoSuchValueInScope();
    }


    public override string ToString()
    {
        return string.Join(Environment.NewLine, _typeCollection.Select(a => $"{a.Key}: {a.Value}"));
    }
}

public class ScopeException : Exception { }

public class NoSuchValueInScope : ScopeException { }
