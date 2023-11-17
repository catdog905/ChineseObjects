namespace ChineseObjects.Lang;


// A return statement. Only stores the expression that is returned.
public class Return : IStatement
{
    public readonly Expression retval;

    public Return(Expression retval)
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