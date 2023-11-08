namespace ChineseObjects.Lang.Entities;

public class Name
{
    public readonly string Value;

    public Name(string value)
    {
        Value = value;
    }

    protected bool Equals(Name other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Name)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Name? left, Name? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Name? left, Name? right)
    {
        return !Equals(left, right);
    }
}