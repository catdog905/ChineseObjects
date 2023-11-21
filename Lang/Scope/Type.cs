namespace ChineseObjects.Lang;

public class Type
{
    private IClassDeclaration _classDeclaration;

    public Type(IClassDeclaration classDeclaration)
    {
        _classDeclaration = classDeclaration;
    }

    public Type(Scope scope, string className) : this(scope.GetType(className)._classDeclaration) {}
    
    public Type(Scope scope, IIdentifier className) : this(scope, className.Name()) {}

    public IIdentifier TypeName()
    {
        return _classDeclaration.ClassName();
    }

    protected bool Equals(Type other)
    {
        return _classDeclaration.Equals(other._classDeclaration);
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
        return _classDeclaration.GetHashCode();
    }

    public static bool operator ==(Type? left, Type? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Type? left, Type? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{_classDeclaration.GetHashCode()}";
    }
}