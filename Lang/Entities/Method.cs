namespace ChineseObjects.Lang.Entities;

public class Method
{
    public readonly MethodName Name;
    public readonly ReturnType ReturnType;

    public Method(MethodName name, ReturnType returnType)
    {
        Name = name;
        ReturnType = returnType;
    }

    public Method(ClassScope scope, MethodDeclaration methodDeclaration)
        : this(new MethodName(methodDeclaration.MethodName), new ReturnType(scope, methodDeclaration.ReturnTypeName)) {}
    

    protected bool Equals(Method other)
    {
        return Name.Equals(other.Name) && ReturnType.Equals(other.ReturnType);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Method)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, ReturnType);
    }

    public static bool operator ==(Method? left, Method? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Method? left, Method? right)
    {
        return !Equals(left, right);
    }
}

public class MethodName
{
    public readonly string value;

    public MethodName(string value)
    {
        this.value = value;
    }

    protected bool Equals(MethodName other)
    {
        return value == other.value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MethodName)obj);
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public static bool operator ==(MethodName? left, MethodName? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(MethodName? left, MethodName? right)
    {
        return !Equals(left, right);
    }
}