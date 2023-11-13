namespace ChineseObjects.Lang;

// Assignment statement
public class Assignment : Statement
{
    public readonly string Varname;
    public readonly Expression Expr;

    public Assignment(string varname, Expression expr)
    {
        Varname = varname;
        Expr = expr;
    }

    public override string ToString()
    {
        return Varname + ":=" + Expr;
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"ASSIGN TO " + Varname + ":"};
        ans.AddRange(Expr.GetRepr().Select(s => "| " + s));
        return ans;
    }
}