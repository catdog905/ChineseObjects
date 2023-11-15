using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IClassDeclaration : IAstNode
{
    public string ClassName();
    public IEnumerable<string> ParentClassNames();
    public IEnumerable<ConstructorDeclaration> ConstructorDeclarations();
    public IEnumerable<VariableDeclaration> VariableDeclarations();
    public IEnumerable<MethodDeclaration> MethodDeclarations();
}

public interface IScopeAwareClassDeclaration : IClassDeclaration
{
    public Scope Scope();
    new IEnumerable<string> ParentClassNames();
    new IEnumerable<ScopeAwareConstructorDeclaration> ConstructorDeclarations();
    new IEnumerable<ScopeAwareVariableDeclaration> VariableDeclarations();
    new IEnumerable<ScopeAwareMethodDeclaration> MethodDeclarations();
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

    public IEnumerable<ConstructorDeclaration> ConstructorDeclarations()
    {
        return _constructorDeclarations;
    }

    public IEnumerable<VariableDeclaration> VariableDeclarations()
    {
        return _variableDeclarations;
    }

    public IEnumerable<MethodDeclaration> MethodDeclarations()
    {
        return _methodDeclarations;
    }
}

public class ScopeAwareClassDeclaration : IClassDeclaration
{
    private readonly Scope _scope;
    private readonly string _className;
    private readonly IEnumerable<string> _parentClassNames;
    private readonly IEnumerable<ConstructorDeclaration> _constructorDeclarations;
    private readonly IEnumerable<VariableDeclaration> _variableDeclarations;
    private readonly IEnumerable<MethodDeclaration> _methodDeclarations;

    private ScopeAwareClassDeclaration(
        ScopeWithFields scope, 
        string className, 
        IEnumerable<string> parentClassNames, 
        IEnumerable<ConstructorDeclaration> constructorDeclarations, 
        IEnumerable<VariableDeclaration> variableDeclarations, 
        IEnumerable<MethodDeclaration> methodDeclarations)
    {
        _scope = scope;
        _className = className;
        _parentClassNames = parentClassNames;
        _constructorDeclarations = constructorDeclarations;
        _variableDeclarations = variableDeclarations;
        _methodDeclarations = methodDeclarations;
    }

    public ScopeAwareClassDeclaration(Scope scope, ClassDeclaration classDeclaration) :
        this(new ScopeWithFields(scope, classDeclaration.VariableDeclarations()),
            classDeclaration.ClassName(),
            classDeclaration.ParentClassNames(),
            classDeclaration.ConstructorDeclarations()
                .Select(decl => new ScopeAwareConstructorDeclaration(scope, decl)).ToImmutableList(),
            classDeclaration.VariableDeclarations()
                .Select(decl => new ScopeAwareVariableDeclaration(scope, decl)).ToImmutableList(),
            classDeclaration.MethodDeclarations()
                .Select(decl => new ScopeAwareMethodDeclaration(scope, decl)).ToImmutableList()
            ) {}

    class ScopeWithFields : Scope
    {
        public ScopeWithFields(Scope scope, IEnumerable<VariableDeclaration> variableDeclarations) :
            base(scope, 
                variableDeclarations.ToDictionary(
                    decl => decl.Name,
                    decl => new Value(decl.Name, new Type(scope, decl.Name)))) {}
    }

    public string ClassName()
    {
        return _className;
    }

    public IEnumerable<string> ParentClassNames()
    {
        return _parentClassNames;
    }

    public IEnumerable<ConstructorDeclaration> ConstructorDeclarations()
    {
        return _constructorDeclarations;
    }

    public IEnumerable<VariableDeclaration> VariableDeclarations()
    {
        return _variableDeclarations;
    }

    public IEnumerable<MethodDeclaration> MethodDeclarations()
    {
        return _methodDeclarations;
    }
}