using System.Collections.Immutable;
using ChineseObjects.Lang.Entities;

namespace ChineseObjects.Lang;

public interface IScope
{
    T? EntityByName<T>(String name) where T : class;
}

public class EmptyScope : IScope
{
    public T? EntityByName<T>(String name) where T : class
    {
        return null;
    }
}

public class Scope<T> : IScope
{
    public readonly IScope OriginScope;
    public readonly ImmutableDictionary<string, T> Values;

    public Scope(IScope originScope, ImmutableDictionary<string, T> values)
    {
        OriginScope = originScope;
        Values = values;
    }

    public Scope(IScope originScope, Dictionary<string, T> classes) : this(originScope, classes.ToImmutableDictionary()) {}
    
    public Scope(Dictionary<string, T> classes) : this(new EmptyScope(), classes.ToImmutableDictionary()) {}

    public Scope(IScope originScope, Scope<T> classScope) : this(originScope, classScope.Values) {}
    public Scope(Scope<T> classScope) : this(new EmptyScope(), classScope.Values) {}

    public TA? EntityByName<TA>(string name) where TA : class 
    {
        // Check if TA is T or a subclass of T
        if (typeof(TA) == typeof(T) || typeof(TA).IsSubclassOf(typeof(T)))
        {
            if (Values.TryGetValue(name, out var value) && value is TA tValue)
            {
                return tValue;
            }
        }

        // Fall back to parent scope if the check fails or the entity is not found
        return OriginScope.EntityByName<TA>(name);
    }
}