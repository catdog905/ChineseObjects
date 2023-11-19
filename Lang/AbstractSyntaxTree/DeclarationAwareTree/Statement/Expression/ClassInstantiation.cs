namespace ChineseObjects.Lang;

public interface IClassInstantiationDeclaration : IClassInstantiation, IExpressionDeclaration
{
    public IIdentifierDeclaration ClassName();
    public IDeclarationArguments Arguments();
}

public class ClassInstantiation : IClassInstantiationDeclaration
{
    private readonly IIdentifierDeclaration _className;
    private readonly IDeclarationArguments _arguments;

    public ClassInstantiation(IIdentifierDeclaration identifier, IDeclarationArguments arguments)
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
        foreach(IArgumentDeclaration arg in _arguments.Values())
        {
            ans.AddRange(arg.Value().GetRepr().Select(s => "| " + s));
        }
        return ans;
    }

    public IIdentifierDeclaration ClassName()
    {
        return _className;
    }

    public IDeclarationArguments Arguments()
    {
        return _arguments;
    }
}

public interface IDeclarationAwareThis : IThis, IExpressionDeclaration {}

public class This : IDeclarationAwareThis
{
    public override string ToString()
    {
        return "This";
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"THIS"};
    }
}