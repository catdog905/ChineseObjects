namespace ChineseObjects.Lang;

public interface IAssignmentDeclaration : IStatementDeclaration {}

// Assignment statement
public class Assignment : IAssignmentDeclaration
{
    public readonly IDeclarationIdentifier Varname;
    public readonly IExpressionDeclaration Expr;

    public Assignment(IDeclarationIdentifier varname, IExpressionDeclaration expr)
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