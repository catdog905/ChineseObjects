namespace ChineseObjects.Lang;

public interface IWhile : IStatement {}

// While statement
public class While : IWhile
{
    public readonly IExpression cond;
    public readonly IStatementsBlock body;

    public While(IExpression cond, IStatementsBlock body)
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