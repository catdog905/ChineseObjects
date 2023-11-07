using System.Collections.Immutable;

namespace ChineseObjects.Lang.Entities;

public class Class
{
    public readonly ClassName Name;
    public readonly ImmutableList<string> MethodNames;
    public readonly ImmutableList<string> FieldNames;
    
    public Class(ClassName className, ImmutableList<string> methodNames, ImmutableList<string> fieldNames)
    {
        Name = className;
        MethodNames = methodNames;
        FieldNames = fieldNames;
    }

    public Class(string className, List<string> methodNames, List<string> fieldNames) :
        this(new ClassName(className), methodNames.ToImmutableList(), fieldNames.ToImmutableList()) {}

    public Class(ClassDeclaration classDeclaration)
        : this(
            new ClassName(classDeclaration.ClassName),
            classDeclaration.MethodDeclarations.Select(decl => decl.MethodName).ToImmutableList(),
            classDeclaration.VariableDeclarations.Select(decl => decl.Name).ToImmutableList())
    {}

    protected bool Equals(Class other)
    {
        return Name.Equals(other.Name) && MethodNames.Equals(other.MethodNames) && FieldNames.Equals(other.FieldNames);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Class)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, MethodNames, FieldNames);
    }

    public static bool operator ==(Class? left, Class? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Class? left, Class? right)
    {
        return !Equals(left, right);
    }
}

public class ClassName
{
    public readonly string Value;

    public ClassName(string value)
    {
        Value = value;
    }

    protected bool Equals(ClassName other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ClassName)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(ClassName? left, ClassName? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ClassName? left, ClassName? right)
    {
        return !Equals(left, right);
    }
}