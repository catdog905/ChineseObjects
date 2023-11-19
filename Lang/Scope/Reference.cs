namespace ChineseObjects.Lang;

public class Reference
{
    public readonly IIdentifier Name;
    public readonly Type Type;

    public Reference(IIdentifier name, Type type)
    {
        Name = name;
        Type = type;
    }

    protected bool Equals(Reference other)
    {
        return Name.Name() == other.Name.Name() && Type.Equals(other.Type);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Reference)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name.Name(), Type);
    }

    public static bool operator ==(Reference? left, Reference? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Reference? left, Reference? right)
    {
        return !Equals(left, right);
    }
}