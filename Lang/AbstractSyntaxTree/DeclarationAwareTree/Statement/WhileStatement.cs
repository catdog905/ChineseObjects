using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement;

public interface IWhile : IStatement
{
    public IExpression Condition();
    public IStatementsBlock Body();
}

// While statement
public class While : IWhile
{
    public readonly IExpression _cond;
    public readonly IStatementsBlock _body;

    public While(IExpression cond, IStatementsBlock body)
    {
        this._cond = cond;
        this._body = body;
    }

    public While(ITypesAwareWhile @while) :
        this(
            new ExpressionWrapper(@while.Condition()),
            new StatementsBlock(@while.Body())) { }

    public override string ToString()
    {
        return "While(" + _cond + "){" + _body + "}";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"WHILE:"};
        ans.AddRange(_cond.GetRepr().Select(s => "| " + s));
        ans.Add("DO:");
        ans.AddRange(_body.GetRepr().Select(s => "| " + s));
        return ans;
    }

    public IExpression Condition()
    {
        return _cond;
    }

    public IStatementsBlock Body()
    {
        return _body;
    }
}