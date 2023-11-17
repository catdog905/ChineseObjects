using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareProgram : IProgram, IScopeAwareAstNode
{
    public new IEnumerable<IScopeAwareClassDeclaration> ClassDeclarations();
}

class ScopeAwareProgram : IScopeAwareProgram
{
    private readonly IEnumerable<IScopeAwareClassDeclaration> _classDeclarations;
    private readonly Scope _scope;

    private ScopeAwareProgram(ScopeWithClassDeclarations scope, IEnumerable<IScopeAwareClassDeclaration> classDeclarations)
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
                        new ScopeAwareClassDeclaration(scope, decl))
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
                    classDeclaration => classDeclaration.ClassName(),
                    classDeclaration => new Type(classDeclaration))) {}
    }

    public Scope Scope()
    {
        return _scope;
    }

    public IEnumerable<IScopeAwareClassDeclaration> ClassDeclarations()
    {
        return _classDeclarations;
    }

    IEnumerable<IClassDeclaration> IProgram.ClassDeclarations()
    {
        return ClassDeclarations();
    }
}