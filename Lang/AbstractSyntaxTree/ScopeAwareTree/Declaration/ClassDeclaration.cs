using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareClass : IScopeAwareAstNode
{
    public IScopeAwareDeclarationIdentifier ClassName();
    public IEnumerable<IScopeAwareDeclarationIdentifier> ParentClassNames();
    public IEnumerable<IScopeAwareConstructor> ConstructorDeclarations();
    public IEnumerable<IScopeAwareVariable> VariableDeclarations();
    public IEnumerable<IScopeAwareMethod> MethodDeclarations();
}

public class ScopeAwareClass : IScopeAwareClass
{
    private readonly Scope _scope;
    private readonly IScopeAwareDeclarationIdentifier _className;
    private readonly IEnumerable<IScopeAwareDeclarationIdentifier> _parentClassNames;
    private readonly IEnumerable<ScopeAwareConstructor> _constructorDeclarations;
    private readonly IEnumerable<ScopeAwareVariable> _variableDeclarations;
    private readonly IEnumerable<ScopeAwareMethod> _methodDeclarations;

    private ScopeAwareClass(
        ScopeWithFields scope, 
        IScopeAwareDeclarationIdentifier className, 
        IEnumerable<IScopeAwareDeclarationIdentifier> parentClassNames, 
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
            new ScopeAwareDeclarationIdentifier(scope, classDeclaration.ClassName()),
            classDeclaration.ParentClassNames()
                .Select(name => new ScopeAwareDeclarationIdentifier(scope, name)),
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


    public IScopeAwareDeclarationIdentifier ClassName()
    {
        return _className;
    }

    public IEnumerable<IScopeAwareDeclarationIdentifier> ParentClassNames()
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