namespace ChineseObjects.Lang;

public interface IAssignment : IStatement {}

// Assignment statement
public class Assignment : IAssignment
{
    public readonly IIdentifier Varname;
    public readonly IExpression Expr;

    public Assignment(IIdentifier varname, IExpression expr)
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