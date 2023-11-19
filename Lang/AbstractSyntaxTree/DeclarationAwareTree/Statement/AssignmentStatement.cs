namespace ChineseObjects.Lang;

public interface IAssignmentDeclaration : IAssignment, IStatementDeclaration {}

// Assignment statement
public class Assignment : IAssignmentDeclaration
{
    public readonly IIdentifierDeclaration Varname;
    public readonly IExpressionDeclaration Expr;

    public Assignment(IIdentifierDeclaration varname, IExpressionDeclaration expr)
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