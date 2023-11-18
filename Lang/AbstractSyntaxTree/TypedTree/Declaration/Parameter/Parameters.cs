using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface ITypesAwareParameters : IParameters, ITypesAwareAstNode
{ 
    public new IEnumerable<ITypedParameter> GetParameters();
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
        _parameters = parameters.GetParameters().Select(parameter => new TypedParameter(parameters.Scope(), parameter));
    }

    IEnumerable<IParameter> IParameters.GetParameters()
    {
        return GetParameters();
    }

    public IEnumerable<ITypedParameter> GetParameters()
    {
        return _parameters;
    }
}