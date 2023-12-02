using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration.Parameter;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration;

public interface IConstructorDeclaration : IMemberDeclaration, IAstNode
{
    public IParameters Parameters();
    public IStatementsBlock Body();
}

public class ConstructorDeclaration : IConstructorDeclaration, IHumanReadable
{
    private readonly Parameters _parameters;
    private readonly StatementsBlock _body;

    public ConstructorDeclaration(Parameters parameters, StatementsBlock statementsBlock)
    {
        _parameters = parameters;
        _body = statementsBlock;
    }

    public override string ToString()
    {
        return "This(" + _parameters + ") {" + _body + "}";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"CONSTRUCTOR"};
        foreach(Parameter.Parameter param in _parameters.GetParameters())
        {
            ans.AddRange(param.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(IStatement stmt in _body._statements) {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }

    public IParameters Parameters()
    {
        return _parameters;
    }

    public IStatementsBlock Body()
    {
        return _body;
    }
}