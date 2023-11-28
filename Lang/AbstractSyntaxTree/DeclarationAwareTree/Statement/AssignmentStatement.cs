namespace ChineseObjects.Lang;

public interface IAssignment : IStatement
{
    public IIdentifier Name();
    public IExpression Expr();
    public IIdentifier TypeName();
}

// Assignment statement
public class Assignment : IAssignment
{
    private readonly IIdentifier _varname;
    private readonly IExpression _expr;
    private readonly IIdentifier _typeName;

    public Assignment(IIdentifier varname, IExpression expr, IIdentifier typeName)
    {
        _varname = varname;
        _expr = expr;
        _typeName = typeName;
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

    public IIdentifier TypeName()
    {
        return _typeName;
    }
}