namespace ChineseObjects.Lang.Declaration;


public interface ITypesAwareMethod : ITypesAwareAstNode
{
    public string MethodName();
    public ITypesAwareParameters Parameters();
    public Type ReturnType();
    public ITypesAwareStatementsBlock Body();
}

public class TypesAwareMethod : ITypesAwareMethod
{
    private readonly string _methodName;
    private readonly ITypesAwareParameters _parameters;
    private readonly Type _returnType;
    private readonly ITypesAwareStatementsBlock _body;

    public TypesAwareMethod(
        string methodName, 
        ITypesAwareParameters parameters, 
        Type returnType, 
        ITypesAwareStatementsBlock body)
    {
        _methodName = methodName;
        _parameters = parameters;
        _returnType = returnType;
        _body = body;
    }
    
    public TypesAwareMethod(IScopeAwareMethod scopeAwareMethod) :
        this(scopeAwareMethod.MethodName().Value(),
            new TypesAwareParameters(scopeAwareMethod.Parameters()),
            new Type(scopeAwareMethod.Scope(), scopeAwareMethod.ReturnTypeName()),
            new TypesAwareStatementsBlock(scopeAwareMethod.Body())) {}

    public string MethodName()
    {
        return _methodName;
    }

    public ITypesAwareParameters Parameters()
    {
        return _parameters;
    }

    public Type ReturnType()
    {
        return _returnType;
    }

    public ITypesAwareStatementsBlock Body()
    {
        return _body;
    }
}