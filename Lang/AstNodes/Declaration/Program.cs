using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IProgram : IAstNode
{
    public IEnumerable<ClassDeclaration> ClassDeclarations();
}

public interface IScopeAwareProgram : IProgram
{
    public Scope Scope();
    new IEnumerable<ScopeAwareClassDeclaration> ClassDeclarations();
}

public class Program : IProgram, IHumanReadable
{
    private readonly ImmutableList<ClassDeclaration> _classDeclarations;

    public Program(IEnumerable<ClassDeclaration> classDeclarations)
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

    public IEnumerable<ClassDeclaration> ClassDeclarations()
    {
        throw new NotImplementedException();
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
    private readonly IEnumerable<ScopeAwareClassDeclaration> _classDeclarations;
    private readonly Scope _scope;

    private ScopeAwareProgram(ScopeWithClassDeclarations scope, IEnumerable<ScopeAwareClassDeclaration> classDeclarations)
    {
        _classDeclarations = classDeclarations;
        _scope = scope;
    }
    
    public ScopeAwareProgram(Scope scope, IProgram program) : 
        this(
            new ScopeWithClassDeclarations(scope, program.ClassDeclarations()), 
            program.ClassDeclarations().Select(
                decl
                    =>
                    new ScopeAwareClassDeclaration(scope, decl))
                .ToList()) {}

    class ScopeWithClassDeclarations : Scope
    {
        public ScopeWithClassDeclarations(Scope scope, IEnumerable<ClassDeclaration> classDeclarations) :
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

    public IEnumerable<ScopeAwareClassDeclaration> ClassDeclarations()
    {
        return _classDeclarations;
    }

    IEnumerable<ClassDeclaration> IProgram.ClassDeclarations()
    {
        throw new NotImplementedException();
    }
}