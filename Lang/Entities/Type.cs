namespace ChineseObjects.Lang.Entities;

public class Type
{
    public readonly Class Class;

    public Type(Class @class)
    {
        Class = @class;
    }
    
    public Type(ClassScope scope, string typeName) : this(GetClassFromScope(scope, typeName)) {}

    private static Class GetClassFromScope(ClassScope scope, string typeName)
    {
        var foundClass = scope.EntityByName<Class>(typeName);
        if (foundClass == null)
        {
            throw new NoSuchValueInScope();
        }
        else
        {
            return foundClass;
        }
    }

    protected bool Equals(Type other)
    {
        return Class.Equals(other.Class);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Type)obj);
    }

    public override int GetHashCode()
    {
        return Class.GetHashCode();
    }

    public static bool operator ==(Type? left, Type? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Type? left, Type? right)
    {
        return !Equals(left, right);
    }
}