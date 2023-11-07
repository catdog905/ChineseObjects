namespace ChineseObjects.Lang.Entities;

public class ReturnType
{
    public readonly Type Type;

    public ReturnType(Type type)
    {
        Type = type;
    }

    public ReturnType(ClassScope scope, string type) : this(new Type(scope, type)) {}

    protected bool Equals(ReturnType other)
    {
        return Type.Equals(other.Type);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ReturnType)obj);
    }

    public override int GetHashCode()
    {
        return Type.GetHashCode();
    }

    public static bool operator ==(ReturnType? left, ReturnType? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ReturnType? left, ReturnType? right)
    {
        return !Equals(left, right);
    }
}