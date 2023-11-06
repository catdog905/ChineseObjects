namespace ChineseObjects.Lang;


// A return statement. Only stores the expression that is returned.
public class Return : Statement
{
    public readonly Expression retval;

    public Return(Expression retval)
    {
        this.retval = retval;
    }

    public override string ToString()
    {
        return retval.ToString();
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"RETURN"};
        ans.AddRange(retval.GetRepr().Select(s => "| " + s));
        return ans;
    }
}