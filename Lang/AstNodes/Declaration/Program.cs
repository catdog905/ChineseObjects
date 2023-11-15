using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IProgram : IAstNode
{
    public IEnumerable<IClassDeclaration> ClassDeclarations();
}

public interface IScopeAwareProgram : IProgram, IScopeAwareAstNode
{
    new IEnumerable<IScopeAwareClassDeclaration> ClassDeclarations();
}

public class Program : IProgram, IHumanReadable
{
    private readonly ImmutableList<IClassDeclaration> _classDeclarations;

    public Program(IEnumerable<IClassDeclaration> classDeclarations)
    {
       _classDeclarations = classDeclarations.ToImmutableList();
    }

    public Program(
        Program program,
        ClassDeclaration classDeclaration
    ) : this(program._classDeclarations.Add(classDeclaration)) {}

    public Program(
        ClassDeclaration classDeclaration,
        Program program
    ) : this(new[] {classDeclaration}.Concat(program.ClassDeclarations())) {}

    public Program(params ClassDeclaration[] classDeclarations) : this(classDeclarations.ToImmutableList()) {}

    public override string ToString()
    {
        return "Program(" + String.Join(", ", _classDeclarations) + ")";
    }

    public IEnumerable<IClassDeclaration> ClassDeclarations()
    {
        return _classDeclarations;
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string>{"PROGRAM"};
        foreach(ClassDeclaration class_ in _classDeclarations)
        {
            ans.AddRange(class_.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
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
        throw new NotImplementedException();
    }
}