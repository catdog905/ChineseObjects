namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

public interface IMethodCall : IExpression
{
    public IExpression Caller();
    public IIdentifier MethodName();
    public IArguments Arguments(); 
}

public class MethodCall : IMethodCall
{
    public readonly IExpression _caller;
    public readonly IIdentifier _methodName;
    public readonly IArguments _arguments;

    public MethodCall(IExpression caller, IIdentifier identifier, IArguments arguments)
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
        foreach (IArgument arg in _arguments.Values())
        {
            ans.AddRange(arg.Value().GetRepr().Select(s => "| " + s));
        }

        return ans;
    }

    public IExpression Caller()
    {
        return _caller;
    }

    public IIdentifier MethodName()
    {
        return _methodName;
    }


    public IArguments Arguments()
    {
        return _arguments;
    }
}
