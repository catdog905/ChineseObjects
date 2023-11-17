using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IClassDeclaration : IAstNode
{
    public string ClassName();
    public IEnumerable<string> ParentClassNames();
    public IEnumerable<IConstructorDeclaration> ConstructorDeclarations();
    public IEnumerable<IVariableDeclaration> VariableDeclarations();
    public IEnumerable<IMethodDeclaration> MethodDeclarations();
}

public class ClassDeclaration : IClassDeclaration, IHumanReadable
{
    private readonly string _className;
    private readonly ImmutableList<string> _parentClassNames;
    private readonly ImmutableList<ConstructorDeclaration> _constructorDeclarations;
    private readonly ImmutableList<VariableDeclaration> _variableDeclarations;
    private readonly ImmutableList<MethodDeclaration> _methodDeclarations;

    public ClassDeclaration(
        string className, 
        ImmutableList<string> parentClassNames, 
        ImmutableList<ConstructorDeclaration> constructorDeclarations, 
        ImmutableList<VariableDeclaration> variableDeclarations, 
        ImmutableList<MethodDeclaration> methodDeclarations)
    {
        _className = className;
        _parentClassNames = parentClassNames;
        _constructorDeclarations = constructorDeclarations;
        _variableDeclarations = variableDeclarations;
        _methodDeclarations = methodDeclarations;
    }
    
    public ClassDeclaration(
        string className, 
        List<string> parentClassNames, 
        List<ConstructorDeclaration> constructorDeclarations, 
        List<VariableDeclaration> variableDeclarations, 
        List<MethodDeclaration> methodDeclarations)
        : this(
            className,
            parentClassNames.ToImmutableList(),
            constructorDeclarations.ToImmutableList(),
            variableDeclarations.ToImmutableList(),
            methodDeclarations.ToImmutableList()) {}

    public ClassDeclaration(
        Identifier className, 
        Identifiers parentClassNames, 
        MemberDeclarations memberDeclarations)
        : this(
            className.Name, 
            parentClassNames.Names,
            ExtractConstructors(memberDeclarations),
            ExtractVariables(memberDeclarations),
            ExtractMethods(memberDeclarations)) {}

    private static ImmutableList<ConstructorDeclaration> ExtractConstructors(MemberDeclarations memberDeclarations)
    {
        var constructors = memberDeclarations.MemberDeclarations_
            .OfType<ConstructorDeclaration>()
            .ToList();

        
        return constructors.ToImmutableList();
    }

    private static ImmutableList<VariableDeclaration> ExtractVariables(MemberDeclarations memberDeclarations)
    {
        var vars = memberDeclarations.MemberDeclarations_
            .OfType<VariableDeclaration>()
            .ToList();
        
        return vars.ToImmutableList();
    }

    private static ImmutableList<MethodDeclaration> ExtractMethods(MemberDeclarations memberDeclarations)
    {
        var methods = memberDeclarations.MemberDeclarations_
            .OfType<MethodDeclaration>()
            .ToList();
        
        return methods.ToImmutableList();
    }
    
    public ClassDeclaration(
        Identifier className, 
        MemberDeclarations memberDeclarations
    ) : this(className, new Identifiers(), memberDeclarations) {}

    public override string ToString()
    {
        return _className + "(vars="+String.Join(", ", _variableDeclarations) + "; methods=" + String.Join(", ", _methodDeclarations) + "), also constructors but we forgot them.";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string>{"CLASS " + _className};
        foreach(ConstructorDeclaration ctor in _constructorDeclarations)
        {
            ans.AddRange(ctor.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(VariableDeclaration var in _variableDeclarations)
        {
            ans.AddRange(var.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(MethodDeclaration method in _methodDeclarations)
        {
            ans.AddRange(method.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }

    public string ClassName()
    {
        return _className;
    }

    public IEnumerable<string> ParentClassNames()
    {
        return _parentClassNames;
    }

    public IEnumerable<IConstructorDeclaration> ConstructorDeclarations()
    {
        return _constructorDeclarations;
    }

    public IEnumerable<IVariableDeclaration> VariableDeclarations()
    {
        return _variableDeclarations;
    }

    public IEnumerable<IMethodDeclaration> MethodDeclarations()
    {
        return _methodDeclarations;
    }
}