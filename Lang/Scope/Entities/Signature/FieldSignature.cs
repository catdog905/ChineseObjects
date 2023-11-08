namespace ChineseObjects.Lang.Entities;

public class FieldSignature
{
    public readonly TypeName TypeName;
    public readonly Name FieldName;

    public FieldSignature(TypeName typeName, Name fieldName)
    {
        TypeName = typeName;
        FieldName = fieldName;
    }
    
    public FieldSignature(VariableDeclaration variableDeclaration)
        : this(new TypeName(variableDeclaration.Type), new Name(variableDeclaration.Name)) {}

    protected bool Equals(FieldSignature other)
    {
        return TypeName.Equals(other.TypeName) && FieldName.Equals(other.FieldName);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FieldSignature)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TypeName, FieldName);
    }

    public static bool operator ==(FieldSignature? left, FieldSignature? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(FieldSignature? left, FieldSignature? right)
    {
        return !Equals(left, right);
    }
}