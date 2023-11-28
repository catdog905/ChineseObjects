namespace ChineseObjects.Lang;

public interface IAssignment : IStatement
{
    public IIdentifier Name();
    public IExpression Expr();
}

// Assignment statement
public class Assignment : IAssignment
{
    private readonly IIdentifier _varname;
    private readonly IExpression _expr;

    public Assignment(IIdentifier varname, IExpression expr)
    {
        _varname = varname;
        _expr = expr;
    }

    public override string ToString()
    {
        return _varname + ":=" + _expr;
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"ASSIGN TO " + _varname + ":"};
        ans.AddRange(_expr.GetRepr().Select(s => "| " + s));
        return ans;
    }

    public IIdentifier Name()
    {
        return _varname;
    }

    public IExpression Expr()
    {
        return _expr;
    }
}