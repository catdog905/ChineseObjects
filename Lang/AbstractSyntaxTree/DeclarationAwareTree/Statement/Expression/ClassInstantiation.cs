namespace ChineseObjects.Lang;

public interface IClassInstantiationDeclaration : IExpressionDeclaration
{
    public IDeclarationIdentifier ClassName();
    public IDeclarationArguments Arguments();
}

public class ClassInstantiation : IClassInstantiationDeclaration
{
    private readonly IDeclarationIdentifier _className;
    private readonly IDeclarationArguments _arguments;

    public ClassInstantiation(IDeclarationIdentifier declarationIdentifier, IDeclarationArguments arguments)
    {
        _className = declarationIdentifier;
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

    public IDeclarationIdentifier ClassName()
    {
        return _className;
    }

    public IDeclarationArguments Arguments()
    {
        return _arguments;
    }
}

public interface IDeclarationAwareThis : IExpressionDeclaration {}

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