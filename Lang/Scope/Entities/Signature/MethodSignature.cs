namespace ChineseObjects.Lang.Entities;

public class MethodSignature
{
    public readonly Name MethodName;
    public readonly SignatureParameters SignatureParameters;
    public readonly TypeName ReturnTypeName;

    public MethodSignature(Name methodName, TypeName returnTypeName, SignatureParameters signatureParameters)
    {
        MethodName = methodName;
        ReturnTypeName = returnTypeName;
        SignatureParameters = signatureParameters;
    }

    public MethodSignature(MethodDeclaration methodDeclaration) : 
        this(
            new Name(methodDeclaration.MethodName),
            new TypeName(methodDeclaration.ReturnTypeName),
            new SignatureParameters(methodDeclaration)) { }

    protected bool Equals(MethodSignature other)
    {
        return MethodName.Equals(other.MethodName) && ReturnTypeName.Equals(other.ReturnTypeName) && SignatureParameters.Equals(other.SignatureParameters);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MethodSignature)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(MethodName, ReturnTypeName, SignatureParameters);
    }

    public static bool operator ==(MethodSignature? left, MethodSignature? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(MethodSignature? left, MethodSignature? right)
    {
        return !Equals(left, right);
    }
}