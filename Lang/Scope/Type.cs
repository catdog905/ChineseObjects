namespace ChineseObjects.Lang;

public class Type
{
    private ClassDeclaration ClassDeclaration;

    public Type(ClassDeclaration classDeclaration)
    {
        ClassDeclaration = classDeclaration;
    }

    public Type(Scope scope, string className) : this(scope.GetType(className).ClassDeclaration) {}

    protected bool Equals(Type other)
    {
        return ClassDeclaration.Equals(other.ClassDeclaration);
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
        return ClassDeclaration.GetHashCode();
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