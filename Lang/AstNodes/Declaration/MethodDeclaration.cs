namespace ChineseObjects.Lang;

public class MethodDeclaration : MemberDeclaration, IHumanReadable
{
    public readonly string MethodName;
    public readonly Parameters Parameters;
    public readonly string ReturnTypeName;
    public readonly StatementsBlock Body;
    
    public MethodDeclaration(string methodName, Parameters parameters, string returnTypeName, StatementsBlock body)
    {
        MethodName = methodName;
        Parameters = parameters;
        ReturnTypeName = returnTypeName;
        Body = body;
    }        
    
    public MethodDeclaration(
        Identifier methodName, 
        Parameters parameters, 
        Identifier returnType, 
        StatementsBlock body) :
        this(methodName.Name, parameters, returnType.Name, body) {}

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

public class ScopeAwareMethodDeclaration : MethodDeclaration
{
    public ScopeAwareMethodDeclaration(
        Scope scope, 
        MethodDeclaration methodDeclaration) : 
        base(methodDeclaration.MethodName, 
            methodDeclaration.Parameters, 
            methodDeclaration.ReturnTypeName, 
            methodDeclaration.Body)
    {
        throw new NotImplementedException();
    }
}