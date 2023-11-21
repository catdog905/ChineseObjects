using System.Collections.Immutable;

namespace ChineseObjects.Lang;

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

    private ScopeAwareProgram(ScopeWithClassDeclarations scope, IProgramDeclaration programDeclaration) : 
        this(
            scope, 
            programDeclaration.ClassDeclarations().Select(
                    decl
                        =>
                        new ScopeAwareClass(scope, decl))
                .ToList()) {}
    
    public ScopeAwareProgram(Scope scope, IProgramDeclaration programDeclaration) : 
        this(
            new ScopeWithClassDeclarations(scope, programDeclaration.ClassDeclarations()), 
            programDeclaration) {}

    class ScopeWithClassDeclarations : Scope
    {
        public ScopeWithClassDeclarations(Scope scope, IEnumerable<IClassDeclaration> classDeclarations) :
            base(
                scope, 
                classDeclarations.ToDictionary(
                    classDeclaration => classDeclaration.ClassName().Name(),
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