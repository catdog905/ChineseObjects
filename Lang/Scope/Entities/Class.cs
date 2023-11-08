using System.Collections.Immutable;

namespace ChineseObjects.Lang.Entities;

public class Class : IType
{
    public readonly Name Name;
    public readonly ImmutableList<ConstructorSignature> Constructors;
    public readonly ImmutableList<MethodSignature> MethodNames;
    public readonly ImmutableList<FieldSignature> FieldNames;
    
    public Class(
        Name className,
        ImmutableList<ConstructorSignature> constructors,
        ImmutableList<MethodSignature> methodNames, 
        ImmutableList<FieldSignature> fieldNames)
    {
        Name = className;
        MethodNames = methodNames;
        FieldNames = fieldNames;
    }

    public Class(string className, List<ConstructorSignature> constructors, List<MethodSignature> methodNames, List<FieldSignature> fieldNames) :
        this(
            new Name(className), 
            constructors.ToImmutableList(),
            methodNames.ToImmutableList(), 
            fieldNames.ToImmutableList()) {}

    public Class(ClassDeclaration classDeclaration)
        : this(
            classDeclaration.ClassName,
            classDeclaration.ConstructorDeclarations.Select(decl => new ConstructorSignature(decl)).ToList(),
            classDeclaration.MethodDeclarations.Select(decl => new MethodSignature(decl)).ToList(),
            classDeclaration.VariableDeclarations.Select(decl => new FieldSignature(decl)).ToList())
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

