using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public class Scope
{
    public readonly ImmutableDictionary<String, Scope> TypeCollection;
    public readonly ImmutableDictionary<String, Scope> ValueCollection;

    public Scope(ImmutableDictionary<string, Scope> typeCollection, ImmutableDictionary<string, Scope> valueCollection)
    {
        TypeCollection = typeCollection;
        ValueCollection = valueCollection;
    }
    
    public Scope(Dictionary<string, Scope> typeCollection, Dictionary<string, Scope> valueCollection) : 
        this(typeCollection.ToImmutableDictionary(), valueCollection.ToImmutableDictionary()) {}

    public Scope GetType(String typeName)
    {
        TypeCollection.TryGetValue(typeName, out Scope value)
        {
            
        }
    }

    public Scope GetValue(String valueName)
    {
        
    }
}