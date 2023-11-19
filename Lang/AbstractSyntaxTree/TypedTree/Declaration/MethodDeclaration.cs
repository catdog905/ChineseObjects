namespace ChineseObjects.Lang.Declaration;


public interface ITypesAwareMethod : IMethod, ITypesAwareAstNode
{
    public IIdentifier MethodName();
    public ITypesAwareParameters Parameters();
    public Type ReturnType();
    public ITypesAwareStatementsBlock Body();
}

public class TypesAwareMethod : ITypesAwareMethod
{
    private readonly IIdentifier _methodName;
    private readonly ITypesAwareParameters _parameters;
    private readonly Type _returnType;
    private readonly ITypesAwareStatementsBlock _body;

    public TypesAwareMethod(
        IIdentifier methodName, 
        ITypesAwareParameters parameters, 
        Type returnType, 
        ITypesAwareStatementsBlock body)
    {
        _methodName = methodName;
        _parameters = parameters;
        _returnType = returnType;
        _body = body;
    }
    
    public TypesAwareMethod(IScopeAwareMethod awareMethod) :
        this(awareMethod.MethodName(),
            new TypesAwareParameters(awareMethod.Parameters()),
            new Type(awareMethod.Scope(), awareMethod.ReturnTypeName()),
            new TypesAwareStatementsBlock(awareMethod.Body())) {}

    public IIdentifier MethodName()
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