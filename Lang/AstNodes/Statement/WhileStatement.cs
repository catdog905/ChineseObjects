namespace ChineseObjects.Lang;

// While statement
public class While : Statement
{
    public readonly Expression cond;
    public readonly Statement body;

    public While(Expression cond, Statement body)
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