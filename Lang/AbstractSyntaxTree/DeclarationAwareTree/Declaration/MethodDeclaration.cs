using System.Reflection.Emit;

namespace ChineseObjects.Lang;

public interface IMethodDeclaration : IMemberDeclaration, IDeclarationAstNode
{
    public IDeclarationIdentifier MethodName();
    public IParameterDeclarations Parameters();
    public IDeclarationIdentifier ReturnTypeName();
    public IDeclarationStatementsBlock Body();
}

public class MethodDeclaration : IMethodDeclaration, IHumanReadable
{
    private readonly IDeclarationIdentifier _methodName;
    private readonly IParameterDeclarations _parameters;
    private readonly IDeclarationIdentifier _returnTypeName;
    private readonly IDeclarationStatementsBlock _body;
    
    public MethodDeclaration(
        IDeclarationIdentifier methodName, 
        IParameterDeclarations parameterDeclarations, 
        IDeclarationIdentifier returnTypeName,
        IDeclarationStatementsBlock body)
    {
        _methodName = methodName;
        _parameters = parameterDeclarations;
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
        foreach(IStatementDeclaration stmt in _body.Statements())
        {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }

    public IDeclarationIdentifier MethodName()
    {
        return _methodName;
    }

    public IParameterDeclarations Parameters()
    {
        return _parameters;
    }

    public IDeclarationIdentifier ReturnTypeName()
    {
        return _returnTypeName;
    }

    public IDeclarationStatementsBlock Body()
    {
        return _body;
    }
}