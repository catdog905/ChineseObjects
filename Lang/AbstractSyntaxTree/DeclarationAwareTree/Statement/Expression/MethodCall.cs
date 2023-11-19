namespace ChineseObjects.Lang;

public interface IMethodCallDeclaration : IMethodCall, IExpressionDeclaration
{
    public IExpressionDeclaration Caller();
    public IIdentifierDeclaration MethodName();
    public IDeclarationArguments Arguments(); 
}

public class MethodCall : IMethodCallDeclaration
{
    public readonly IExpressionDeclaration _caller;
    public readonly IIdentifierDeclaration _methodName;
    public readonly IDeclarationArguments _arguments;

    public MethodCall(IExpressionDeclaration caller, IIdentifierDeclaration identifier, IDeclarationArguments arguments)
    {
        _caller = caller;
        _methodName = identifier;
        _arguments = arguments;
    }

    public override string ToString()
    {
        return "MethodCall(" + _caller + "." + _methodName + "(" + _arguments + "))";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> { "CALL METHOD OF:" };
        ans.AddRange(_caller.GetRepr().Select(s => "| " + s));
        ans.Add("NAMED " + _methodName);
        ans.Add("WITH ARGS:");
        foreach (IArgumentDeclaration arg in _arguments.Values())
        {
            ans.AddRange(arg.Value().GetRepr().Select(s => "| " + s));
        }

        return ans;
    }

    public IExpressionDeclaration Caller()
    {
        return _caller;
    }

    public IIdentifierDeclaration MethodName()
    {
        return _methodName;
    }


    public IDeclarationArguments Arguments()
    {
        return _arguments;
    }
}
