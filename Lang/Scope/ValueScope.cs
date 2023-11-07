using System.Collections.Immutable;
using ChineseObjects.Lang.Entities;

namespace ChineseObjects.Lang;

public class ValueScope : IScope
{
    public readonly IScope OriginScope;
    public readonly ImmutableDictionary<string, Value> Values;

    public ValueScope(IScope originScope, ImmutableDictionary<string, Value> values)
    {
        OriginScope = originScope;
        Values = values;
    }
    
    public ValueScope(IScope originScope, Dictionary<string, Value> values) : this(originScope, values.ToImmutableDictionary()) {}
    
    public ValueScope(IScope originScope, ValueScope valueScope) : this(originScope, valueScope.Values) {}
    
    public ValueScope(ValueScope valueScope) : this(new EmptyScope(), valueScope.Values) {}
    

    public T? EntityByName<T>(string name) where T : class
    {
        // Check if T is Class or a subclass of Class
        if (typeof(T) == typeof(Value) || typeof(T).IsSubclassOf(typeof(Value)))
        {
            if (Values.TryGetValue(name, out var value) && value is T tValue)
            {
                return tValue;
            }
        }

        // Fall back to parent scope if the check fails or the class is not found
        return OriginScope.EntityByName<T>(name);
    }
}