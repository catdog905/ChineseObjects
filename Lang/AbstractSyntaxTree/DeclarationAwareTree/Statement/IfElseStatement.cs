namespace ChineseObjects.Lang;

public interface IIfElseDeclaration : IIfElse, IStatementDeclaration {}

// If-then[-else] statement
public class IfElse : IIfElseDeclaration
{
    public readonly IExpressionDeclaration cond;
    public readonly IDeclarationStatementsBlock then;
    public readonly IDeclarationStatementsBlock? else_;

    public IfElse(IExpressionDeclaration cond, IDeclarationStatementsBlock then, IDeclarationStatementsBlock? else_)
    {
        this.cond = cond;
        this.then = then;
        this.else_ = else_;
            
    }
        
    public IfElse(IExpressionDeclaration cond, IDeclarationStatementsBlock then)
    {
        this.cond = cond;
        this.then = then;
    }

    public override string ToString()
    {
        return "IfElse(" + cond + "){" + then + "}{" + else_ + "}";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"IF:"};
        ans.AddRange(cond.GetRepr().Select(s => "| " + s));
        ans.Add("THEN:");
        ans.AddRange(((StatementsBlock)then).GetRepr().Select(s => "| " + s));
        if (else_ is not null)
        {
            ans.Add("ELSE:");
            ans.AddRange(((StatementsBlock)else_).GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}