using System.Collections.Immutable;
using ChineseObjects.Lang.Entities;

namespace ChineseObjects.Lang;

public class MethodScope : IScope
{
    public readonly IScope OriginScope;
    public readonly ImmutableDictionary<string, Method> Methods;
    
    public MethodScope(IScope originScope, ImmutableDictionary<string, Method> methods)
    {
        OriginScope = originScope;
        Methods = methods;
    }

    public MethodScope(IScope originScope, Dictionary<string, Method> methods) : this(originScope, methods.ToImmutableDictionary()) {}
    
    public MethodScope(Dictionary<string, Method> methods) : this(new EmptyScope(), methods.ToImmutableDictionary()) {}

    public MethodScope(IScope originScope, MethodScope methodScope) : this(originScope, methodScope.Methods) {}
    public MethodScope(MethodScope methodScope) : this(new EmptyScope(), methodScope.Methods) {}

    public T? EntityByName<T>(string name) where T : class
    {
        // Check if T is Method or a subclass of Method
        if (typeof(T) == typeof(Method) || typeof(T).IsSubclassOf(typeof(Method)))
        {
            if (Methods.TryGetValue(name, out var value) && value is T tValue)
            {
                return tValue;
            }
        }

        // Fall back to parent scope if the check fails or the method is not found
        return OriginScope.EntityByName<T>(name);
    }

    
}