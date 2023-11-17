using System.Reflection.Emit;

namespace ChineseObjects.Lang;


public interface IScopeAwareMethodDeclaration : IMethodDeclaration, IScopeAwareAstNode
{
    public new IScopeAwareParameters Parameters();
    public new IScopeAwareStatementsBlock Body();
}

public class ScopeAwareMethodDeclaration : IScopeAwareMethodDeclaration
{
    private readonly Scope _scope;
    private readonly string _methodName;
    private readonly IScopeAwareParameters _parameters;
    private readonly string _returnTypeName;
    private readonly IScopeAwareStatementsBlock _body;

    public ScopeAwareMethodDeclaration(
        Scope scope, 
        string methodName, 
        IScopeAwareParameters parameters, 
        string returnTypeName, 
        IScopeAwareStatementsBlock body)
    {
        _scope = scope;
        _methodName = methodName;
        _parameters = parameters;
        _returnTypeName = returnTypeName;
        _body = body;
    }

    public ScopeAwareMethodDeclaration(Scope scope, IMethodDeclaration methodDeclaration) :
        this(scope,
            methodDeclaration.MethodName(),
            new ScopeAwareParameters(scope, methodDeclaration.Parameters()),
            methodDeclaration.ReturnTypeName(),
            new ScopeAwareStatementsBlock(scope, methodDeclaration.Body())) {}


    public string MethodName()
    {
        return _methodName;
    }

    public IParameters Parameters()
    {
        return _parameters;
    }

    public IScopeAwareStatementsBlock Body()
    {
        return _body;
    }

    public string ReturnTypeName()
    {
        return _returnTypeName;
    }

    IStatementsBlock IMethodDeclaration.Body()
    {
        return Body();
    }

    IScopeAwareParameters IScopeAwareMethodDeclaration.Parameters()
    {
        return _parameters;
    }

    public Scope Scope()
    {
        return _scope;
    }
}