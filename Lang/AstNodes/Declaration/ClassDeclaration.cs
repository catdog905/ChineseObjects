using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public class ClassDeclaration : IAstNode, IHumanReadable
{
    public readonly string ClassName;
    public readonly ImmutableList<string> ParentClassNames;
    public readonly ImmutableList<ConstructorDeclaration> ConstructorDeclarations;
    public readonly ImmutableList<VariableDeclaration> VariableDeclarations;
    public readonly ImmutableList<MethodDeclaration> MethodDeclarations;

    public ClassDeclaration(
        string className, 
        ImmutableList<string> parentClassNames, 
        ImmutableList<ConstructorDeclaration> constructorDeclarations, 
        ImmutableList<VariableDeclaration> variableDeclarations, 
        ImmutableList<MethodDeclaration> methodDeclarations)
    {
        ClassName = className;
        ParentClassNames = parentClassNames;
        ConstructorDeclarations = constructorDeclarations;
        VariableDeclarations = variableDeclarations;
        MethodDeclarations = methodDeclarations;
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
        return ClassName + "(vars="+String.Join(", ", VariableDeclarations) + "; methods=" + String.Join(", ", MethodDeclarations) + "), also constructors but we forgot them.";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string>{"CLASS " + ClassName};
        foreach(ConstructorDeclaration ctor in ConstructorDeclarations)
        {
            ans.AddRange(ctor.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(VariableDeclaration var in VariableDeclarations)
        {
            ans.AddRange(var.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(MethodDeclaration method in MethodDeclarations)
        {
            ans.AddRange(method.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}

public class ScopeAwareClassDeclaration : ClassDeclaration
{
    private readonly ClassDeclaration Origin;
    private readonly Scope Scope;

    private ScopeAwareClassDeclaration(ScopeWithFields scope, ClassDeclaration classDeclaration) :
        base(classDeclaration.ClassName,
            classDeclaration.ParentClassNames,
            classDeclaration.ConstructorDeclarations
                .Select(decl => new ScopeAwareConstructorDeclaration(scope, decl)),
            classDeclaration.VariableDeclarations
                .Select(decl => new ScopeAwareVariableDeclaration(scope, decl)),
            classDeclaration.MethodDeclarations
                .Select(decl => new ScopeAwareMethodDeclaration(scope, decl)))
    {
        Origin = classDeclaration;
        Scope = scope;
    }
    
    public ScopeAwareClassDeclaration(Scope scope, ClassDeclaration classDeclaration) :
        this(new ScopeWithFields(scope, classDeclaration.VariableDeclarations), classDeclaration) {}

    class ScopeWithFields : Scope
    {
        public ScopeWithFields(Scope scope, IEnumerable<VariableDeclaration> variableDeclarations) :
            base(scope, 
                variableDeclarations.ToDictionary(
                    decl => decl.Name,
                    decl => new Value(/*TODO*/))) {}
    }
}