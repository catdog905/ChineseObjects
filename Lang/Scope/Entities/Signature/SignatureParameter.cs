using System.Collections.Immutable;

namespace ChineseObjects.Lang.Entities;

public class SignatureParameters
{
    public readonly ImmutableList<SignatureParameter> Values;

    public SignatureParameters(ImmutableList<SignatureParameter> values)
    {
        Values = values;
    }
    
    public SignatureParameters(Parameters parameters)
        : this(parameters.Parames.Select(parameter => new SignatureParameter(parameter)).ToImmutableList()) {}

    public SignatureParameters(MethodDeclaration methodDeclaration)
        : this(methodDeclaration.Parameters) {}
    
    public SignatureParameters(ConstructorDeclaration constructorDeclaration)
        : this(constructorDeclaration.Parameters) {}

    protected bool Equals(SignatureParameters other)
    {
        return Values.Equals(other.Values);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SignatureParameters)obj);
    }

    public override int GetHashCode()
    {
        return Values.GetHashCode();
    }

    public static bool operator ==(SignatureParameters? left, SignatureParameters? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SignatureParameters? left, SignatureParameters? right)
    {
        return !Equals(left, right);
    }
}

public class SignatureParameter
{
    public readonly Name ParameterName;
    public readonly TypeName ParameterTypeName;

    public SignatureParameter(Name parameterName, TypeName parameterTypeName)
    {
        ParameterName = parameterName;
        ParameterTypeName = parameterTypeName;
    }
    
    public SignatureParameter(Parameter parameter)
        : this(
            new Name(parameter.Name), 
            new TypeName(parameter.Type)) {}

    protected bool Equals(SignatureParameter other)
    {
        return ParameterName.Equals(other.ParameterName) && ParameterTypeName.Equals(other.ParameterTypeName);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SignatureParameter)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ParameterName, ParameterTypeName);
    }

    public static bool operator ==(SignatureParameter? left, SignatureParameter? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SignatureParameter? left, SignatureParameter? right)
    {
        return !Equals(left, right);
    }
}