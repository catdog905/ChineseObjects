using System.Reflection.Emit;

namespace ChineseObjects.Lang;


public interface IScopeAwareMethod : IScopeAwareAstNode
{
    public IScopeAwareDeclarationIdentifier MethodName();
    public IScopeAwareParameters Parameters();
    public IScopeAwareDeclarationIdentifier ReturnTypeName();
    public IScopeAwareStatementsBlock Body();
}

public class ScopeAwareMethod : IScopeAwareMethod
{
    private readonly Scope _scope;
    private readonly IScopeAwareDeclarationIdentifier _methodName;
    private readonly IScopeAwareParameters _parameters;
    private readonly IScopeAwareDeclarationIdentifier _returnTypeName;
    private readonly IScopeAwareStatementsBlock _body;

    public ScopeAwareMethod(
        Scope scope, 
        IScopeAwareDeclarationIdentifier methodName, 
        IScopeAwareParameters parameters, 
        IScopeAwareDeclarationIdentifier returnTypeName, 
        IScopeAwareStatementsBlock body)
    {
        _scope = scope;
        _methodName = methodName;
        _parameters = parameters;
        _returnTypeName = returnTypeName;
        _body = body;
    }

    public ScopeAwareMethod(Scope scope, IMethodDeclaration methodDeclaration) :
        this(scope,
            new ScopeAwareDeclarationIdentifier(scope, methodDeclaration.MethodName()),
            new ScopeAwareParameters(scope, methodDeclaration.Parameters()),
            new ScopeAwareDeclarationIdentifier(scope, methodDeclaration.ReturnTypeName()),
            new ScopeAwareStatementsBlock(scope, methodDeclaration.Body())) {}


    public IScopeAwareDeclarationIdentifier MethodName()
    {
        return _methodName;
    }

    public IScopeAwareStatementsBlock Body()
    {
        return _body;
    }

    public IScopeAwareDeclarationIdentifier ReturnTypeName()
    {
        return _returnTypeName;
    }

    IScopeAwareParameters IScopeAwareMethod.Parameters()
    {
        return _parameters;
    }

    public Scope Scope()
    {
        return _scope;
    }
}