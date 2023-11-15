namespace ChineseObjects.Lang;

// While statement
public class While : IStatement
{
    public readonly Expression cond;
    public readonly IStatement body;

    public While(Expression cond, IStatement body)
    {
        this.cond = cond;
        this.body = body;
    }

    public override string ToString()
    {
        return "While(" + cond + "){" + body + "}";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"WHILE:"};
        ans.AddRange(cond.GetRepr().Select(s => "| " + s));
        ans.Add("DO:");
        ans.AddRange(body.GetRepr().Select(s => "| " + s));
        return ans;
    }
}