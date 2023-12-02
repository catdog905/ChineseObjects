using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement;

public interface IReturn : IStatement
{
    public IExpression Value();
}

// A return statement. Only stores the expression that is returned.
public class Return : IReturn
{
    private readonly IExpression _retval;

    public Return(IExpression retval)
    {
        _retval = retval;
    }

    public override string ToString()
    {
        return _retval?.ToString() ?? "NULL";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> { "RETURN" };
        ans.AddRange(_retval.GetRepr().Select(s => "| " + s));
        return ans;
    }

    public IExpression Value()
    {
        return _retval;
    }
}