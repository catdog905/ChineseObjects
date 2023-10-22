using System.Collections.Immutable;

namespace ChineseObjects.Lang
{
    public abstract class Declaration {}
    
    public class Program
    {
        public readonly List<ClassDeclaration> ClassDeclarations;

        public Program(List<ClassDeclaration> classDeclarations)
        {
            ClassDeclarations = classDeclarations;
        }

        public Program(params ClassDeclaration[] classDeclarations)
        {
            ClassDeclarations = classDeclarations.ToList();
        }

        public override string ToString()
        {
            return "Program(" + String.Join(", ", ClassDeclarations) + ")";
        }
    }
    
    public class ClassDeclaration : Declaration
    {
        public readonly string ClassName;

        public ClassDeclaration(string className)
        {
            ClassName = className;
        }

        public override string ToString()
        {
            return ClassName;
        }
    }
    
    // A parameter (is not an expression)
    public class Parameter {
        public readonly string name;
        public readonly Expression type;

        public Parameter(string name, Expression type) {
            this.name = name;
            this.type = type;
        }
    }

    // A list of parameters (is not an expression)
    public class Parameters {
        public readonly ImmutableList<Parameter> parames;

        public Parameters(ImmutableList<Parameter> parames) {
            this.parames = parames;
        }

        public Parameters() : this(ImmutableList<Parameter>.Empty) {}

        public Parameters Append(Parameter param) {
            return new Parameters(parames.Add(param));
        }
    }

    // public class MethodDeclaration : Declaration {
    //     public readonly string name;
    //     public readonly Parameters parames;
    //     public readonly ... body;
    // }
}