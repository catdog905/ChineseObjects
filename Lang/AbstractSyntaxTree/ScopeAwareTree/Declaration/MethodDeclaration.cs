using System.Reflection.Emit;

namespace ChineseObjects.Lang;


public interface IScopeAwareMethod : IScopeAwareAstNode
{
    public IScopeAwareIdentifier MethodName();
    public IScopeAwareParameters Parameters();
    public IScopeAwareIdentifier ReturnTypeName();
    public IScopeAwareStatementsBlock Body();
}

public class ScopeAwareMethod : IScopeAwareMethod
{
    private readonly Scope _scope;
    private readonly IScopeAwareIdentifier _methodName;
    private readonly IScopeAwareParameters _parameters;
    private readonly IScopeAwareIdentifier _returnTypeName;
    private readonly IScopeAwareStatementsBlock _body;

    private ScopeAwareMethod(
        ScopeWithParameters scope, 
        IScopeAwareIdentifier methodName, 
        IScopeAwareParameters parameters, 
        IScopeAwareIdentifier returnTypeName, 
        IScopeAwareStatementsBlock body)
    {
        _scope = scope;
        _methodName = methodName;
        _parameters = parameters;
        _returnTypeName = returnTypeName;
        _body = body;
    }

    private ScopeAwareMethod(ScopeWithParameters scope, IMethodDeclaration methodDeclaration) :
        this(scope,
            new ScopeAwareIdentifier(scope, methodDeclaration.MethodName()),
            new ScopeAwareParameters(scope, methodDeclaration.Parameters()),
            new ScopeAwareIdentifier(scope, methodDeclaration.ReturnTypeName()),
            new ScopeAwareStatementsBlock(scope, methodDeclaration.Body())) {}

    public ScopeAwareMethod(Scope scope, IMethodDeclaration methodDeclaration) :
        this(
            new ScopeWithParameters(scope, methodDeclaration.Parameters().GetParameters()),
            methodDeclaration) {}
    
    class ScopeWithParameters : Scope
    {
        public ScopeWithParameters(Scope scope, IEnumerable<IParameterDeclaration> parameters) :
            base(scope, 
                parameters.ToDictionary(
                    parameter => parameter.Name().Value(),
                    parameter => new Entity(parameter.Name(), new Type(scope, parameter.TypeName())))
            ) {}
    }

    public IScopeAwareIdentifier MethodName()
    {
        return _methodName;
    }

    public IScopeAwareStatementsBlock Body()
    {
        return _body;
    }

    public IScopeAwareIdentifier ReturnTypeName()
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