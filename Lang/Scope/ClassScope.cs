using System.Collections.Immutable;
using ChineseObjects.Lang.Entities;

namespace ChineseObjects.Lang;

public class ClassScope : IScope
{
    public readonly IScope OriginScope;
    public readonly ImmutableDictionary<string, Class> Classes;

    public ClassScope(IScope originScope, ImmutableDictionary<string, Class> classes)
    {
        OriginScope = originScope;
        Classes = classes;
    }

    public ClassScope(IScope originScope, Dictionary<string, Class> classes) : this(originScope, classes.ToImmutableDictionary()) {}
    
    public ClassScope(Dictionary<string, Class> classes) : this(new EmptyScope(), classes.ToImmutableDictionary()) {}

    public ClassScope(IScope originScope, ClassScope classScope) : this(originScope, classScope.Classes) {}
    public ClassScope(ClassScope classScope) : this(new EmptyScope(), classScope.Classes) {}

    public T? EntityByName<T>(string name) where T : class
    {
        // Check if T is Class or a subclass of Class
        if (typeof(T) == typeof(Class) || typeof(T).IsSubclassOf(typeof(Class)))
        {
            if (Classes.TryGetValue(name, out var value) && value is T tValue)
            {
                return tValue;
            }
        }

        // Fall back to parent scope if the check fails or the class is not found
        return OriginScope.EntityByName<T>(name);
    }

}