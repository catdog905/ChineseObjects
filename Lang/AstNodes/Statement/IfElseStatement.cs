namespace ChineseObjects.Lang;

// If-then[-else] statement
public class IfElse : IStatement
{
    public readonly Expression cond;
    public readonly IStatement then;
    public readonly IStatement? else_;

    public IfElse(Expression cond, IStatement then, IStatement? else_)
    {
        this.cond = cond;
        this.then = then;
        this.else_ = else_;
            
    }
        
    public IfElse(Expression cond, IStatement then)
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
        ans.AddRange(then.GetRepr().Select(s => "| " + s));
        if (else_ is not null)
        {
            ans.Add("ELSE:");
            ans.AddRange(else_.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}