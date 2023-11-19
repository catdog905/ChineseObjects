using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IProgramDeclaration : IProgram, IDeclarationAstNode
{
    public IEnumerable<IClassDeclaration> ClassDeclarations();
}

public class Program : IProgramDeclaration, IHumanReadable
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