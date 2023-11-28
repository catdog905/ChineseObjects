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
}