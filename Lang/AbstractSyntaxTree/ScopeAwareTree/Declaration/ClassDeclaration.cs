using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareClassDeclaration : IClassDeclaration, IScopeAwareAstNode
{
    public new IEnumerable<IScopeAwareConstructorDeclaration> ConstructorDeclarations();
    public new IEnumerable<IScopeAwareVariableDeclaration> VariableDeclarations();
    public new IEnumerable<IScopeAwareMethodDeclaration> MethodDeclarations();
}

public class ScopeAwareClassDeclaration : IScopeAwareClassDeclaration
{
    private readonly Scope _scope;
    private readonly string _className;
    private readonly IEnumerable<string> _parentClassNames;
    private readonly IEnumerable<ScopeAwareConstructorDeclaration> _constructorDeclarations;
    private readonly IEnumerable<ScopeAwareVariableDeclaration> _variableDeclarations;
    private readonly IEnumerable<ScopeAwareMethodDeclaration> _methodDeclarations;

    private ScopeAwareClassDeclaration(
        ScopeWithFields scope, 
        string className, 
        IEnumerable<string> parentClassNames, 
        IEnumerable<ScopeAwareConstructorDeclaration> constructorDeclarations, 
        IEnumerable<ScopeAwareVariableDeclaration> variableDeclarations, 
        IEnumerable<ScopeAwareMethodDeclaration> methodDeclarations)
    {
        _scope = scope;
        _className = className;
        _parentClassNames = parentClassNames;
        _constructorDeclarations = constructorDeclarations;
        _variableDeclarations = variableDeclarations;
        _methodDeclarations = methodDeclarations;
    }
    
    private ScopeAwareClassDeclaration(ScopeWithFields scope, IClassDeclaration classDeclaration) :
        this(scope,
            classDeclaration.ClassName(),
            classDeclaration.ParentClassNames(),
            classDeclaration.ConstructorDeclarations()
                .Select(decl => new ScopeAwareConstructorDeclaration(scope, decl)).ToImmutableList(),
            classDeclaration.VariableDeclarations()
                .Select(decl => new ScopeAwareVariableDeclaration(scope, decl)).ToImmutableList(),
            classDeclaration.MethodDeclarations()
                .Select(decl => new ScopeAwareMethodDeclaration(scope, decl)).ToImmutableList()
        ) {}


    public ScopeAwareClassDeclaration(Scope scope, IClassDeclaration classDeclaration) :
        this(new ScopeWithFields(scope, classDeclaration.VariableDeclarations()),
            classDeclaration) {}

    class ScopeWithFields : Scope
    {
        public ScopeWithFields(Scope scope, IEnumerable<IVariableDeclaration> variableDeclarations) :
            base(scope, 
                variableDeclarations.ToDictionary(
                    decl => decl.Name(),
                    decl => new Reference(decl.Name(), new Type(scope, decl.Type())))) {}
    }


    public string ClassName()
    {
        return _className;
    }

    public IEnumerable<string> ParentClassNames()
    {
        throw new NotImplementedException();
    }

    IEnumerable<IConstructorDeclaration> IClassDeclaration.ConstructorDeclarations()
    {
        return ConstructorDeclarations();
    }

    IEnumerable<IVariableDeclaration> IClassDeclaration.VariableDeclarations()
    {
        return VariableDeclarations();
    }

    IEnumerable<IMethodDeclaration> IClassDeclaration.MethodDeclarations()
    {
        return MethodDeclarations();
    }

    public IEnumerable<IScopeAwareConstructorDeclaration> ConstructorDeclarations()
    {
        return _constructorDeclarations;
    }

    public IEnumerable<IScopeAwareVariableDeclaration> VariableDeclarations()
    {
        return _variableDeclarations;
    }

    public IEnumerable<IScopeAwareMethodDeclaration> MethodDeclarations()
    {
        return _methodDeclarations;
    }

    public Scope Scope()
    {
        return _scope;
    }
}