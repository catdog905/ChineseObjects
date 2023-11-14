using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public class Program : IAstNode, IHumanReadable
{
    public readonly ImmutableList<ClassDeclaration> ClassDeclarations;

    public Program(IEnumerable<ClassDeclaration> classDeclarations)
    {
        ClassDeclarations = classDeclarations.ToImmutableList();
    }

    public Program(
        Program program,
        ClassDeclaration classDeclaration
    ) : this(program.ClassDeclarations.Add(classDeclaration)) {}

    public Program(
        ClassDeclaration classDeclaration,
        Program program
    ) : this(new[] {classDeclaration}.Concat(program.ClassDeclarations)) {}

    public Program(params ClassDeclaration[] classDeclarations) : this(classDeclarations.ToImmutableList()) {}

    public override string ToString()
    {
        return "Program(" + String.Join(", ", ClassDeclarations) + ")";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string>{"PROGRAM"};
        foreach(ClassDeclaration class_ in ClassDeclarations)
        {
            ans.AddRange(class_.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}


class ScopeAwareProgram : Program
{
    private readonly Program Origin;
    private readonly Scope Scope;

    private ScopeAwareProgram(ScopeWithClassDeclarations scope, Program program) :
        base(program.ClassDeclarations.Select(
            decl
                =>
                new ScopeAwareClassDeclaration(scope, decl)))
    {
        Origin = program;
        Scope = scope;
    }
    
    public ScopeAwareProgram(Scope scope, Program program) : 
        this(new ScopeWithClassDeclarations(scope, program.ClassDeclarations), program) {}

    class ScopeWithClassDeclarations : Scope
    {
        public ScopeWithClassDeclarations(Scope scope, IEnumerable<ClassDeclaration> classDeclarations) :
            base(
                scope, 
                classDeclarations.ToDictionary(
                    classDeclaration => classDeclaration.ClassName,
                    classDeclaration => new Type(classDeclaration))) {}
    }
}