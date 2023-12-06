using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration.Parameter;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration.Parameter;

public interface IParameterDeclaration
{
    public IIdentifier Name();
    public IIdentifier TypeName();
}
    
// A parameter (is not an expression)
public class Parameter : IParameterDeclaration, IHumanReadable {
    private readonly IIdentifier _name;
    private readonly IIdentifier _typeName;

    public Parameter(IIdentifier name, IIdentifier typeName) {
        _name = name;
        _typeName = typeName;
    }

    public Parameter(TypedParameter parameter) :
        this(new Identifier(parameter.Name()), parameter.Type().TypeName()) { }

    public Parameter(ITypedParameter parameter) :
        this(new Identifier(parameter.Name()),
            parameter.Type().TypeName()) {}

    public override string ToString()
    {
        return _name + ": " + _typeName;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"PARAMETER " + _name + ": " + _typeName};
    }


    public IIdentifier Name()
    {
        return _name;
    }

    public IIdentifier TypeName()
    {
        return _typeName;
    }
}