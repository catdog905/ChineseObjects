namespace ChineseObjects.Lang;

public interface IReturn : IStatement {}

// A return statement. Only stores the expression that is returned.
public class Return : IReturn
{
    public readonly IExpression retval;

    public Return(IExpression retval)
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