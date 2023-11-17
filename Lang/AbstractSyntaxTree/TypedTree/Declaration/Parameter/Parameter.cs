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
    
    public TypedParameter(Scope scope, string name, string typeName) : this(name, typeName,scope.GetType(typeName))
    {}
    
    public TypedParameter(Scope scope, IParameter parameter) : this(scope, parameter.Name(), parameter.TypeName()) {}


    public string Name() => _name;

    public string TypeName() => _typeName;

    public Type Type() => _type;
}