namespace ChineseObjects.Lang;

public class MethodCall : Expression
{
    public readonly Expression Caller;
    public readonly string MethodName;
    public readonly Arguments Arguments;

    public MethodCall(Expression caller, Identifier identifier, Arguments arguments)
    {
        Caller = caller;
        MethodName = identifier.Name;
        Arguments = arguments;
    }

    public override string ToString()
    {
        return "MethodCall(" + Caller + "." + MethodName + "(" + Arguments + "))";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> { "CALL METHOD OF:" };
        ans.AddRange(Caller.GetRepr().Select(s => "| " + s));
        ans.Add("NAMED " + MethodName);
        ans.Add("WITH ARGS:");
        foreach (Argument arg in Arguments.Values)
        {
            ans.AddRange(arg.Value.GetRepr().Select(s => "| " + s));
        }

        return ans;
    }
}
