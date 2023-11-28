namespace ChineseObjects.Lang;

public interface IClassInstantiation : IExpression
{
    public IIdentifier ClassName();
    public IArguments Arguments();
}

public class ClassInstantiation : IClassInstantiation
{
    private readonly IIdentifier _className;
    private readonly IArguments _arguments;

    public ClassInstantiation(IIdentifier identifier, IArguments arguments)
    {
        _className = identifier;
        _arguments = arguments;
    }

    public override string ToString()
    {
        return "new " + _className + "(" + _arguments + ")";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"NEW " + _className};
        foreach(IArgument arg in _arguments.Values())
        {
            ans.AddRange(arg.Value().GetRepr().Select(s => "| " + s));
        }
        return ans;
    }

    public IIdentifier ClassName()
    {
        return _className;
    }

    public IArguments Arguments()
    {
        return _arguments;
    }
}
