using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface ITypesAwareParameters : ITypesAwareAstNode
{ 
    public IEnumerable<ITypedParameter> GetParameters();
}

public class TypesAwareParameters : ITypesAwareParameters
{
    private readonly IEnumerable<ITypedParameter> _parameters;

    public TypesAwareParameters(IEnumerable<ITypedParameter> parameters)
    {
        _parameters = parameters;
    }
    
    public TypesAwareParameters(IScopeAwareParameters parameters)
    {
        _parameters = parameters.GetParameters().Select(parameter => new TypedParameter(parameter));
    }

    public IEnumerable<ITypedParameter> GetParameters()
    {
        return _parameters;
    }

    protected bool Equals(TypesAwareParameters other)
    {
        if (_parameters.Count() != other.GetParameters().Count())
            return false;
        var parametersList = _parameters.ToList();
        var otherParametersList = other.GetParameters().ToList();
        for (int i = 0; i < _parameters.Count(); i++)
        {
            if (!parametersList[i].Type().Equals(otherParametersList[i].Type()))
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TypesAwareParameters)obj);
    }

    public override int GetHashCode()
    {
        return _parameters.Aggregate(0, (acc, parameter) => acc + parameter.Type().GetHashCode());
    }

    public static bool operator ==(TypesAwareParameters? left, TypesAwareParameters? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TypesAwareParameters? left, TypesAwareParameters? right)
    {
        return !Equals(left, right);
    }
}