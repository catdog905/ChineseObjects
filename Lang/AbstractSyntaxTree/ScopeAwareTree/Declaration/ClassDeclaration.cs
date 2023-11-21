using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareClass : IScopeAwareAstNode
{
    public IScopeAwareIdentifier ClassName();
    public IEnumerable<IScopeAwareIdentifier> ParentClassNames();
    public IEnumerable<IScopeAwareConstructor> ConstructorDeclarations();
    public IEnumerable<IScopeAwareVariable> VariableDeclarations();
    public IEnumerable<IScopeAwareMethod> MethodDeclarations();
}

public class ScopeAwareClass : IScopeAwareClass
{
    private readonly Scope _scope;
    private readonly IScopeAwareIdentifier _className;
    private readonly IEnumerable<IScopeAwareIdentifier> _parentClassNames;
    private readonly IEnumerable<ScopeAwareConstructor> _constructorDeclarations;
    private readonly IEnumerable<ScopeAwareVariable> _variableDeclarations;
    private readonly IEnumerable<ScopeAwareMethod> _methodDeclarations;

    private ScopeAwareClass(
        ScopeWithFields scope, 
        IScopeAwareIdentifier className, 
        IEnumerable<IScopeAwareIdentifier> parentClassNames, 
        IEnumerable<ScopeAwareConstructor> constructorDeclarations, 
        IEnumerable<ScopeAwareVariable> variableDeclarations, 
        IEnumerable<ScopeAwareMethod> methodDeclarations)
    {
        _scope = scope;
        _className = className;
        _parentClassNames = parentClassNames;
        _constructorDeclarations = constructorDeclarations;
        _variableDeclarations = variableDeclarations;
        _methodDeclarations = methodDeclarations;
    }
    
    private ScopeAwareClass(ScopeWithFields scope, IClassDeclaration classDeclaration) :
        this(scope,
            new ScopeAwareIdentifier(scope, classDeclaration.ClassName()),
            classDeclaration.ParentClassNames()
                .Select(name => new ScopeAwareIdentifier(scope, name)),
            classDeclaration.ConstructorDeclarations()
                .Select(decl => new ScopeAwareConstructor(scope, decl)).ToImmutableList(),
            classDeclaration.VariableDeclarations()
                .Select(decl => new ScopeAwareVariable(scope, decl)).ToImmutableList(),
            classDeclaration.MethodDeclarations()
                .Select(decl => new ScopeAwareMethod(scope, decl)).ToImmutableList()
        ) {}


    public ScopeAwareClass(Scope scope, IClassDeclaration classDeclaration) :
        this(new ScopeWithFields(scope, classDeclaration.VariableDeclarations()),
            classDeclaration) {}

    class ScopeWithFields : Scope
    {
        public ScopeWithFields(Scope scope, IEnumerable<IVariableDeclaration> variableDeclarations) :
            base(scope, 
                variableDeclarations.ToDictionary(
                    decl => decl.Name().Name(),
                    decl => new Reference(decl.Name(), new Type(scope, decl.TypeName())))) {}
    }


    public IScopeAwareIdentifier ClassName()
    {
        return _className;
    }

    public IEnumerable<IScopeAwareIdentifier> ParentClassNames()
    {
        return _parentClassNames;
    }

    public IEnumerable<IScopeAwareConstructor> ConstructorDeclarations()
    {
        return _constructorDeclarations;
    }

    public IEnumerable<IScopeAwareVariable> VariableDeclarations()
    {
        return _variableDeclarations;
    }

    public IEnumerable<IScopeAwareMethod> MethodDeclarations()
    {
        return _methodDeclarations;
    }

    public Scope Scope()
    {
        return _scope;
    }
}