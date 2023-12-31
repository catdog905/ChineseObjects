using System.Collections.Immutable;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;
using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Declaration;

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
        var scopeAwareIdentifiers = parentClassNames.ToList();
        _parentClassNames = scopeAwareIdentifiers;
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
        this(new ScopeWithFields(scope,
                classDeclaration.VariableDeclarations().ToImmutableList()
                    .Add(new VariableDeclaration(new Identifier("this"), classDeclaration.ClassName()))),
            classDeclaration) {}

    class ScopeWithFields : Scope
    {
        public ScopeWithFields(Scope scope, IEnumerable<IVariableDeclaration> variableDeclarations) :
            base(scope, 
                variableDeclarations.ToDictionary(
                    decl => decl.Identifier().Value(),
                    decl => new Entity(decl.Identifier(), new Type(scope, decl.TypeName())))) {}
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

public interface IScopeAwareThis : IScopeAwareExpression {}

public class ScopeAwareThis : IScopeAwareThis
{
    private readonly Scope _scope;

    public ScopeAwareThis(Scope scope)
    {
        _scope = scope;
    }

    public Scope Scope()
    {
        return _scope;
    }
}
