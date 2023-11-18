namespace ChineseObjects.Lang.Declaration;


public interface ITypesAwareMethodDeclaration : IMethodDeclaration, ITypesAwareAstNode
{
    public new ITypesAwareParameters Parameters();
    public new Type ReturnType();
    public new ITypesAwareStatementsBlock Body();
}

public class TypesAwareMethodDeclaration : ITypesAwareMethodDeclaration
{
    private readonly string _methodName;
    private readonly ITypesAwareParameters _parameters;
    private readonly Type _returnType;
    private readonly ITypesAwareStatementsBlock _body;

    public TypesAwareMethodDeclaration(
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
    
    public TypesAwareMethodDeclaration(IScopeAwareMethodDeclaration methodDeclaration) :
        this(methodDeclaration.MethodName(),
            new TypesAwareParameters(methodDeclaration.Parameters()),
            new Type(methodDeclaration.Scope(), methodDeclaration.ReturnTypeName()),
            new TypesAwareStatementsBlock(methodDeclaration.Body())) {}

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

    IParameters IMethodDeclaration.Parameters()
    {
        return Parameters();
    }

    public string ReturnTypeName()
    {
        return _returnType.TypeName();
    }

    IStatementsBlock IMethodDeclaration.Body()
    {
        return Body();
    }
}