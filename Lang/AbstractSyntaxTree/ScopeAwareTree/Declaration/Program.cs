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

    private ScopeAwareProgram(ScopeWithClassDeclarations scope, IProgram program) : 
        this(
            scope, 
            program.ClassDeclarations().Select(
                    decl
                        =>
                        new ScopeAwareClass(scope, decl))
                .ToList()) {}
    
    public ScopeAwareProgram(Scope scope, IProgram program) : 
        this(
            new ScopeWithClassDeclarations(scope, program.ClassDeclarations()), 
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