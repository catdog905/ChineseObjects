namespace ChineseObjects.Lang.Entities;

public class ConstructorSignature
{
    public readonly SignatureParameters SignatureParameters;

    public ConstructorSignature(SignatureParameters signatureParameters)
    {
        SignatureParameters = signatureParameters;
    }

    public ConstructorSignature(ConstructorDeclaration constructorDeclaration) : 
        this(new SignatureParameters(constructorDeclaration.Parameters)) {}

    protected bool Equals(ConstructorSignature other)
    {
        return SignatureParameters.Equals(other.SignatureParameters);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ConstructorSignature)obj);
    }

    public override int GetHashCode()
    {
        return SignatureParameters.GetHashCode();
    }

    public static bool operator ==(ConstructorSignature? left, ConstructorSignature? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ConstructorSignature? left, ConstructorSignature? right)
    {
        return !Equals(left, right);
    }
}
