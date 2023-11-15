using System.Reflection.Emit;

namespace ChineseObjects.Lang;

public interface IMethodDeclaration : MemberDeclaration
{
    public string MethodName();
    public Parameters Parameters();
    public string ReturnTypeName();
    public StatementsBlock Body();
}

public interface IScopeAwareMethodDeclaration : IMethodDeclaration, IScopeAwareAstNode
{
    public string MethodName();
    public ScopeAwareParameters Parameters();
    public string ReturnTypeName();
    public ScopeAwareStatementsBlock Body();
}

public class MethodDeclaration : IMethodDeclaration, IHumanReadable
{
    private readonly string _methodName;
    private readonly Parameters _parameters;
    private readonly string _returnTypeName;
    private readonly StatementsBlock _body;
    
    public MethodDeclaration(string methodName, Parameters parameters, string returnTypeName, StatementsBlock body)
    {
        _methodName = methodName;
        _parameters = parameters;
        _returnTypeName = returnTypeName;
        _body = body;
    }        
    
    public MethodDeclaration(
        Identifier methodName, 
        Parameters parameters, 
        Identifier returnType, 
        StatementsBlock body) :
        this(methodName.Name, parameters, returnType.Name, body) {}

    public override string ToString()
    {
        return _methodName + "(" + _parameters + "):" + _returnTypeName + "{" + _body + "}";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string>{"METHOD " + _methodName + ": " + _returnTypeName};
        foreach(Parameter param in _parameters.GetParameters())
        {
            ans.AddRange(param.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(IStatement stmt in _body._statements)
        {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }

    public string MethodName()
    {
        return _methodName;
    }

    public Parameters Parameters()
    {
        return _parameters;
    }

    public string ReturnTypeName()
    {
        return _returnTypeName;
    }

    public StatementsBlock Body()
    {
        return _body;
    }
}

public class ScopeAwareMethodDeclaration : IScopeAwareMethodDeclaration
{
    private readonly Scope _scope;
    private readonly string _methodName;
    private readonly ScopeAwareParameters _parameters;
    private readonly string _returnTypeName;
    private readonly ScopeAwareStatementsBlock _body;

    public ScopeAwareMethodDeclaration(
        Scope scope, 
        string methodName, 
        ScopeAwareParameters parameters, 
        string returnTypeName, 
        ScopeAwareStatementsBlock body)
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

    string IMethodDeclaration.MethodName()
    {
        throw new NotImplementedException();
    }

    ScopeAwareParameters IScopeAwareMethodDeclaration.Parameters()
    {
        return _parameters;
    }

    string IScopeAwareMethodDeclaration.ReturnTypeName()
    {
        return _returnTypeName;
    }

    ScopeAwareStatementsBlock IScopeAwareMethodDeclaration.Body()
    {
        return _body;
    }

    string IScopeAwareMethodDeclaration.MethodName()
    {
        return _methodName;
    }

    Parameters IMethodDeclaration.Parameters()
    {
        throw new NotImplementedException();
    }

    string IMethodDeclaration.ReturnTypeName()
    {
        throw new NotImplementedException();
    }

    StatementsBlock IMethodDeclaration.Body()
    {
        throw new NotImplementedException();
    }

    public Scope Scope()
    {
        return _scope;
    }
}