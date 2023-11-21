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
}