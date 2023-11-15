namespace ChineseObjects.Lang;

public class Value
{
    public readonly string Name;
    public readonly Type Type;

    public Value(string name, Type type)
    {
        Name = name;
        Type = type;
    }

    protected bool Equals(Value other)
    {
        return Name == other.Name && Type.Equals(other.Type);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Value)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Type);
    }

    public static bool operator ==(Value? left, Value? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Value? left, Value? right)
    {
        return !Equals(left, right);
    }
}