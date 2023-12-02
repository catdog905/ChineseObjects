using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement;

public interface IIfElse : IStatement
{
    public IExpression Condition();
    public IStatementsBlock Then();
    public IStatementsBlock? Else();
}

// If-then[-else] statement
public class IfElse : IIfElse
{
    private readonly IExpression _cond;
    private readonly IStatementsBlock _then;
    private readonly IStatementsBlock? _else;

    public IfElse(IExpression cond, IStatementsBlock then, IStatementsBlock? else_)
    {
        _cond = cond;
        _then = then;
        _else = else_;
            
    }
        
    public IfElse(IExpression cond, IStatementsBlock then) :
        this(cond, then, null) {}

    public override string ToString()
    {
        return "IfElse(" + _cond + "){" + _then + "}{" + _else + "}";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"IF:"};
        ans.AddRange(_cond.GetRepr().Select(s => "| " + s));
        ans.Add("THEN:");
        ans.AddRange(((StatementsBlock)_then).GetRepr().Select(s => "| " + s));
        if (_else is not null)
        {
            ans.Add("ELSE:");
            ans.AddRange(((StatementsBlock)_else).GetRepr().Select(s => "| " + s));
        }
        return ans;
    }

    public IExpression Condition()
    {
        return _cond;
    }

    public IStatementsBlock Then()
    {
        return _then;
    }

    public IStatementsBlock? Else()
    {
        return _else;
    }
}