using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface ITypedParameter : ITypedAstNode
{
    public string Name();
}

public class TypedParameter : ITypedParameter
{
    private readonly string _name;
    private readonly Type _type;

    public TypedParameter(string name, Type type)
    {
        _name = name;
        _type = type;
    }
    
    public TypedParameter(IScopeAwareParameter parameter) :
        this(
            parameter.Name().Value(), 
            parameter.Scope().GetType(parameter.TypeName().Value())) {}


    public string Name() => _name;

    public Type Type() => _type;

    protected bool Equals(TypedParameter other)
    {
        return _name == other._name && _type.Equals(other._type);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TypedParameter)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_name, _type);
    }

    public static bool operator ==(TypedParameter? left, TypedParameter? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TypedParameter? left, TypedParameter? right)
    {
        return !Equals(left, right);
    }
    
    public T AcceptVisitor<T>(CodeGen.ITypesAwareStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}