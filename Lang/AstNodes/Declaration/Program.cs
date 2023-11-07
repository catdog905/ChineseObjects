using System.Collections.Immutable;
using System.Formats.Asn1;
using ChineseObjects.Lang.Entities;

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
    public readonly Program Program;
    public readonly IScope Scope;
    
    public ScopeAwareProgram(Program program) : base(program.ClassDeclarations)
    {
        Program = program;
        Scope = new ClassScope(
            program.ClassDeclarations.ToDictionary(
                    classDeclaration => classDeclaration.ClassName, 
                    classDeclaration => new Class(classDeclaration)));
    }
}