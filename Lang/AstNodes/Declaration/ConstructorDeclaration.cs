namespace ChineseObjects.Lang;

public class ConstructorDeclaration : MemberDeclaration, IHumanReadable
{
    public readonly Parameters Parameters;
    public readonly StatementsBlock Body;

    public ConstructorDeclaration(Parameters parameters, StatementsBlock statementsBlock)
    {
        Parameters = parameters;
        Body = statementsBlock;
    }

    public override string ToString()
    {
        return "This(" + Parameters + ") {" + Body + "}";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"CONSTRUCTOR"};
        foreach(Parameter param in Parameters.Parames)
        {
            ans.AddRange(param.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(Statement stmt in Body.Stmts) {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}
//
// public class ScopeConstructorDeclaration : ConstructorDeclaration
// {
//     public readonly IScope Scope;
//     public readonly ConstructorDeclaration ConstructorDeclaration;
//
//     public ScopeConstructorDeclaration(IScope scope, ConstructorDeclaration constructorDeclaration) 
//         : base(constructorDeclaration.Parameters, constructorDeclaration.Body)
//     {
//         var parametersScope = 
//     }
// }