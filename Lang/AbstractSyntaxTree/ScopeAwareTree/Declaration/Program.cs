using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration;

namespace ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Declaration;

public interface IScopeAwareProgram : IScopeAwareAstNode
{
    public IEnumerable<IScopeAwareClass> ClassDeclarations();
}

class ScopeAwareProgram : IScopeAwareProgram
{
    private readonly IEnumerable<IScopeAwareClass> _classDeclarations;
    private readonly Scope _scope;

    private ScopeAwareProgram(ScopeWithClassDeclarations scope, IEnumerable<IScopeAwareClass> classDeclarations)
    {
        _classDeclarations = classDeclarations;
        _scope = scope;
    }

    private ScopeAwareProgram(ScopeWithClassDeclarations scope, Scope originalScope,
        IEnumerable<IClassDeclaration> declarationsWithInheritedMethods) : 
        this(
            new ScopeWithClassDeclarations(scope, declarationsWithInheritedMethods), 
            declarationsWithInheritedMethods
                .Select(
                    decl
                        =>
                        new ScopeAwareClass(
                            new ScopeWithClassDeclarations(scope, declarationsWithInheritedMethods),
                            decl))
                .ToList()) {}
    
    private ScopeAwareProgram(ScopeWithClassDeclarations scope, Scope originalScope, IProgram program) : 
        this(
            scope,
            originalScope,
            program.ClassDeclarations()
                .Select(decl => ClassDeclaration.WithInheritedMethods(decl, scope))
            ) {}
    
    public ScopeAwareProgram(Scope scope, IProgram program) : 
        this(
            new ScopeWithClassDeclarations(scope, program.ClassDeclarations()), 
            scope,
            program) {}

    
    class ScopeWithClassDeclarations : Scope
    {
        public ScopeWithClassDeclarations(Scope scope, IEnumerable<IClassDeclaration> classDeclarations) :
            base(
                scope, 
                classDeclarations.ToDictionary(
                    classDeclaration => classDeclaration.ClassName().Value(),
                    classDeclaration => new Type(classDeclaration))) {}
    }

    public Scope Scope()
    {
        return _scope;
    }

    public IEnumerable<IScopeAwareClass> ClassDeclarations()
    {
        return _classDeclarations;
    }
}