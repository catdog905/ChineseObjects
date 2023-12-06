using System.Collections.Immutable;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration;

public interface IProgram : IAstNode
{
    public IEnumerable<IClassDeclaration> ClassDeclarations();
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

    public Program(ITypesAwareProgram program) :
        this(program.ClassDeclarations()
            .Select(decl => new ClassDeclaration(decl)))
    {
    }

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