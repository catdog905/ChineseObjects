namespace ChineseObjects.Lang;

public interface IReturnDeclaration : IStatementDeclaration {}

// A return statement. Only stores the expression that is returned.
public class Return : IReturnDeclaration
{
    public readonly IExpressionDeclaration retval;

    public Return(IExpressionDeclaration retval)
    {
        this.retval = retval;
    }

    public override string ToString()
    {
        return retval?.ToString() ?? "NULL";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> { "RETURN" };
        ans.AddRange(retval.GetRepr().Select(s => "| " + s));
        return ans;
    }
}