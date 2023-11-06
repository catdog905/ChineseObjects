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