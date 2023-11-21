using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IClassDeclaration : IDeclarationAstNode
{
    public IDeclarationIdentifier ClassName();
    public IEnumerable<IDeclarationIdentifier> ParentClassNames();
    public IEnumerable<IConstructorDeclaration> ConstructorDeclarations();
    public IEnumerable<IVariableDeclaration> VariableDeclarations();
    public IEnumerable<IMethodDeclaration> MethodDeclarations();
}

public class ClassDeclaration : IClassDeclaration, IHumanReadable
{
    private readonly IDeclarationIdentifier _className;
    private readonly ImmutableList<IDeclarationIdentifier> _parentClassNames;
    private readonly ImmutableList<IConstructorDeclaration> _constructorDeclarations;
    private readonly ImmutableList<IVariableDeclaration> _variableDeclarations;
    private readonly ImmutableList<IMethodDeclaration> _methodDeclarations;

    public ClassDeclaration(
        IDeclarationIdentifier className, 
        ImmutableList<IDeclarationIdentifier> parentClassNames, 
        ImmutableList<IConstructorDeclaration> constructorDeclarations, 
        ImmutableList<IVariableDeclaration> variableDeclarations, 
        ImmutableList<IMethodDeclaration> methodDeclarations)
    {
        _className = className;
        _parentClassNames = parentClassNames;
        _constructorDeclarations = constructorDeclarations;
        _variableDeclarations = variableDeclarations;
        _methodDeclarations = methodDeclarations;
    }
    
    public ClassDeclaration(
        IDeclarationIdentifier className, 
        IEnumerable<IDeclarationIdentifier> parentClassNames, 
        IEnumerable<IConstructorDeclaration> constructorDeclarations, 
        IEnumerable<IVariableDeclaration> variableDeclarations, 
        IEnumerable<IMethodDeclaration> methodDeclarations)
        : this(
            className,
            parentClassNames.ToImmutableList(),
            constructorDeclarations.ToImmutableList(),
            variableDeclarations.ToImmutableList(),
            methodDeclarations.ToImmutableList()) {}

    public ClassDeclaration(
        IDeclarationIdentifier className, 
        IDeclarationIdentifiers parentClassNames, 
        MemberDeclarations memberDeclarations)
        : this(
            className, 
            parentClassNames.GetIdentifiers(),
            ExtractConstructors(memberDeclarations),
            ExtractVariables(memberDeclarations),
            ExtractMethods(memberDeclarations)) {}

    private static IEnumerable<IConstructorDeclaration> ExtractConstructors(MemberDeclarations memberDeclarations)
    {
        var constructors = memberDeclarations.MemberDeclarations_
            .OfType<IConstructorDeclaration>()
            .ToList();

        
        return constructors.ToImmutableList();
    }

    private static IEnumerable<IVariableDeclaration> ExtractVariables(MemberDeclarations memberDeclarations)
    {
        var vars = memberDeclarations.MemberDeclarations_
            .OfType<IVariableDeclaration>()
            .ToList();
        
        return vars.ToImmutableList();
    }

    private static IEnumerable<IMethodDeclaration> ExtractMethods(MemberDeclarations memberDeclarations)
    {
        var methods = memberDeclarations.MemberDeclarations_
            .OfType<IMethodDeclaration>()
            .ToList();
        
        return methods.ToImmutableList();
    }
    
    public ClassDeclaration(
        IDeclarationIdentifier className, 
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

    public IDeclarationIdentifier ClassName()
    {
        return _className;
    }

    public IEnumerable<IDeclarationIdentifier> ParentClassNames()
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