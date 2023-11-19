using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface ITypedParameter : IParameter, ITypedAstNode {}

public class TypedParameter : ITypedParameter
{
    private readonly string _name;
    private readonly string _typeName;
    private readonly Type _type;

    public TypedParameter(string name, string typeName, Type type)
    {
        _name = name;
        _typeName = typeName;
        _type = type;
    }
    
    public TypedParameter(IScopeAwareParameter parameter) :
        this(
            parameter.Name().Name(), 
            parameter.TypeName().Name(), 
            parameter.Scope().GetType(parameter.TypeName().Name())) {}


    public string Name() => _name;

    public string TypeName() => _typeName;

    public Type Type() => _type;
}