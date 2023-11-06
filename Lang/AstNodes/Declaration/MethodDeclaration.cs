namespace ChineseObjects.Lang;

public class MethodDeclaration : MemberDeclaration, IHumanReadable
{
    public readonly string MethodName;
    public readonly Parameters Parameters;
    public readonly string ReturnTypeName;
    public readonly StatementsBlock Body;


    public MethodDeclaration(
        Identifier methodName, 
        Parameters parameters, 
        Identifier returnType, 
        StatementsBlock body)
    {
        MethodName = methodName.Name;
        Parameters = parameters;
        ReturnTypeName = returnType.Name;
        Body = body;
    }

    public override string ToString()
    {
        return MethodName + "(" + Parameters + "):" + ReturnTypeName + "{" + Body + "}";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string>{"METHOD " + MethodName + ": " + ReturnTypeName};
        foreach(Parameter param in Parameters.Parames)
        {
            ans.AddRange(param.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(Statement stmt in Body.Stmts)
        {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}