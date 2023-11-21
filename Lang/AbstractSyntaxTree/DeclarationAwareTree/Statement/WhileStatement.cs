namespace ChineseObjects.Lang;

public interface IWhileDeclaration : IStatementDeclaration {}

// While statement
public class While : IWhileDeclaration
{
    public readonly IExpressionDeclaration cond;
    public readonly IStatementDeclaration body;

    public While(IExpressionDeclaration cond, IStatementDeclaration body)
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