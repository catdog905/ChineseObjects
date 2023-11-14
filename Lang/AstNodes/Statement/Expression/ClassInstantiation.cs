namespace ChineseObjects.Lang;

public class ClassInstantiation : Expression
{
    public readonly string ClassName;
    public readonly Arguments Arguments;

    public ClassInstantiation(Identifier identifier, Arguments arguments)
    {
        ClassName = identifier.Name;
        Arguments = arguments;
    }

    public ClassDeclaration? EvaluatedType(Scope scope) {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "new " + ClassName + "(" + Arguments + ")";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"NEW " + ClassName};
        foreach(Argument arg in Arguments.Values)
        {
            ans.AddRange(arg.Value.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}

public class This : Expression
{
    public ClassDeclaration? EvaluatedType(Scope scope) {
        throw new NotImplementedException();
    }
    
    public override string ToString()
    {
        return "This";
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"THIS"};
    }
}
