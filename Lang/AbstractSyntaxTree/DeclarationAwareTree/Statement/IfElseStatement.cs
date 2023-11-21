namespace ChineseObjects.Lang;

public interface IIfElse : IStatement {}

// If-then[-else] statement
public class IfElse : IIfElse
{
    public readonly IExpression cond;
    public readonly IStatementsBlock then;
    public readonly IStatementsBlock? else_;

    public IfElse(IExpression cond, IStatementsBlock then, IStatementsBlock? else_)
    {
        this.cond = cond;
        this.then = then;
        this.else_ = else_;
            
    }
        
    public IfElse(IExpression cond, IStatementsBlock then)
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