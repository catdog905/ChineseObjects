using System.Reflection.Emit;

namespace ChineseObjects.Lang;

public interface IMethodDeclaration : IMemberDeclaration, IAstNode
{
    public IIdentifier MethodName();
    public IParameter Parameters();
    public IIdentifier ReturnTypeName();
    public IStatementsBlock Body();
}

public class MethodDeclaration : IMethodDeclaration, IHumanReadable
{
    private readonly IIdentifier _methodName;
    private readonly IParameter _parameters;
    private readonly IIdentifier _returnTypeName;
    private readonly IStatementsBlock _body;
    
    public MethodDeclaration(
        IIdentifier methodName, 
        IParameter parameter, 
        IIdentifier returnTypeName,
        IStatementsBlock body)
    {
        _methodName = methodName;
        _parameters = parameter;
        _returnTypeName = returnTypeName;
        _body = body;
    }        
    
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
        foreach(IStatement stmt in _body.Statements())
        {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }

    public IIdentifier MethodName()
    {
        return _methodName;
    }

    public IParameter Parameters()
    {
        return _parameters;
    }

    public IIdentifier ReturnTypeName()
    {
        return _returnTypeName;
    }

    public IStatementsBlock Body()
    {
        return _body;
    }
}