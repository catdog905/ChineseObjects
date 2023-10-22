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

        public Program(ClassDeclaration classDeclaration, Program program)
        {
            ClassDeclarations = new List<ClassDeclaration> {classDeclaration};
            ClassDeclarations.AddRange(program.ClassDeclarations);
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
        public readonly List<string> ParentClassNames;
        public readonly List<VariableDeclaration> VariableDeclarations = new List<VariableDeclaration> {};
        public readonly List<MethodDeclaration> MethodDeclarations = new List<MethodDeclaration> {};

        public ClassDeclaration(
            Identifier className, 
            Identifiers parentClassNames, 
            MemberDeclarations memberDeclarations)
        {
            ClassName = className.name;
            ParentClassNames = parentClassNames.Names;
            foreach (MemberDeclaration memberDeclaration in memberDeclarations.MemberDeclarationsList)
            {
                switch (memberDeclaration)
                {
                    case VariableDeclaration varDeclaration:
                        VariableDeclarations.Add(varDeclaration);
                        break;
                    case MethodDeclaration methodDeclaration:
                        MethodDeclarations.Add(methodDeclaration);
                        break;
                }
            }
        }
        
        public ClassDeclaration(
            Identifier className, 
            MemberDeclarations memberDeclarations) : this(className, new Identifiers(), memberDeclarations) {}

        public override string ToString()
        {
            return ClassName + "(vars="+String.Join(", ", VariableDeclarations) + "; methods=" + String.Join(", ", MethodDeclarations) + ")";
        }
    }

    public abstract class MemberDeclaration {}

    public class MemberDeclarations
    {
        public readonly List<MemberDeclaration> MemberDeclarationsList;

        public MemberDeclarations()
        {
            MemberDeclarationsList = new List<MemberDeclaration> ();
        }
        
        public MemberDeclarations(MemberDeclaration memberDeclaration)
        {
            MemberDeclarationsList = new List<MemberDeclaration> { memberDeclaration };
        }
        
        public MemberDeclarations(MemberDeclaration memberDeclaration, MemberDeclarations memberDeclarations)
        {
            MemberDeclarationsList = new List<MemberDeclaration> { memberDeclaration };
            MemberDeclarationsList.AddRange(memberDeclarations.MemberDeclarationsList);
        }
    }
    
    public class MethodDeclaration : MemberDeclaration
    {
        public readonly string MethodName;
        public readonly Parameters Parameters;
        public readonly string ReturnTypeName;
        public readonly StatementsBlock body;


        public MethodDeclaration(
            Identifier methodName, 
            Parameters parameters, 
            Identifier returnType, 
            StatementsBlock body)
        {
            MethodName = methodName.name;
            Parameters = parameters;
            ReturnTypeName = returnType.name;
            this.body = body;
        }

        public override string ToString()
        {
            return MethodName + "(" + Parameters + "):" + ReturnTypeName;
        }
    }
    
    // A variable declaration (is not an expression)
    // TODO: should declarations of initialized and uninitialized variable
    // be the same or different types of nodes?
    public class VariableDeclaration : MemberDeclaration
    {
        public readonly string name;
        public readonly Identifier Type;

        public VariableDeclaration(Identifier name, Identifier type)
        {
            this.name = name.name;
            this.Type = type;
        }

        public override string ToString()
        {
            return name + ":" + Type;
        }
    }


    public class ConstructorDeclaration : MemberDeclaration
    {
        //TODO
    }
    
    //abs public class MethodDeclaration : Declaration {
    //     public readonly string name;
    //     public readonly Parameters parames;
    //     public readonly ... body;
    // }
    
    // A parameter (is not an expression)
    public class Parameter {
        public readonly string name;
        public readonly Expression type;

        public Parameter(string name, Expression type) {
            this.name = name;
            this.type = type;
        }

        public override string ToString()
        {
            return name;
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

        public override string ToString()
        {
            return String.Join(",", parames);
        }
    }
}