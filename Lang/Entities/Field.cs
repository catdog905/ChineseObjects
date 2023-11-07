namespace ChineseObjects.Lang.Entities;

public class Field
{
    public readonly FieldName Name;
    public readonly Type Type;

    public Field(FieldName name, Type type)
    {
        Name = name;
        Type = type;
    }

    protected bool Equals(Field other)
    {
        return Name.Equals(other.Name) && Type.Equals(other.Type);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Field)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Type);
    }

    public static bool operator ==(Field? left, Field? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Field? left, Field? right)
    {
        return !Equals(left, right);
    }
}

public class FieldName
{
    public readonly string Value;

    public FieldName(string value)
    {
        Value = value;
    }

    protected bool Equals(FieldName other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FieldName)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(FieldName? left, FieldName? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(FieldName? left, FieldName? right)
    {
        return !Equals(left, right);
    }
}