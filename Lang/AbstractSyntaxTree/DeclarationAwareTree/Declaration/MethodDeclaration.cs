using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration.Parameter;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration;

public interface IMethodDeclaration : IMemberDeclaration, IAstNode
{
    public IIdentifier MethodName();
    public IParameters Parameters();
    public IIdentifier ReturnTypeName();
    public IStatementsBlock Body();
    public string Signature();
}

public class MethodDeclaration : IMethodDeclaration, IHumanReadable
{
    private readonly IIdentifier _methodName;
    private readonly IParameters _parameters;
    private readonly IIdentifier _returnTypeName;
    private readonly IStatementsBlock _body;
    
    public MethodDeclaration(
        IIdentifier methodName, 
        IParameters parameters, 
        IIdentifier returnTypeName,
        IStatementsBlock body)
    {
        _methodName = methodName;
        _parameters = parameters;
        _returnTypeName = returnTypeName;
        _body = body;
    }

    public MethodDeclaration(ITypesAwareMethod method) :
        this(new Identifier(method.MethodName()),
            new Parameters(method.Parameters()),
            method.ReturnType().TypeName(),
            new StatementsBlock(method.Body())
            )
    {}

    public override string ToString()
    {
        return _methodName + "(" + _parameters + "):" + _returnTypeName + "{" + _body + "}";
    }

    public string Signature()
    {
        return _methodName + "(" + String.Join(",", _parameters.GetParameters().Select(parameter => parameter.TypeName()))  + ")";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string>{"METHOD " + _methodName + ": " + _returnTypeName};
        foreach(Parameter.Parameter param in _parameters.GetParameters())
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

    public IParameters Parameters()
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