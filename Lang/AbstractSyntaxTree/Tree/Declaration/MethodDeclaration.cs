using System.Reflection.Emit;

namespace ChineseObjects.Lang;

public interface IMethodDeclaration : MemberDeclaration
{
    public string MethodName();
    public IParameters Parameters();
    public string ReturnTypeName();
    public IStatementsBlock Body();
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

    public IParameters Parameters()
    {
        return _parameters;
    }

    public string ReturnTypeName()
    {
        return _returnTypeName;
    }

    public IStatementsBlock Body()
    {
        return _body;
    }
}