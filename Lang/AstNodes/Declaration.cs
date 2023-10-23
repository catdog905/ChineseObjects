using System.Collections.Immutable;

namespace ChineseObjects.Lang
{
    public interface Declaration : IAstNode {}

    public class Program : IAstNode
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

        public List<IAstNode> Children()
        {
            return ClassDeclarations.Cast<IAstNode>().ToList();
        }

        public IAstNode CurrentNode()
        {
            return this;
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

        public List<IAstNode> Children()
        {
            List<IAstNode> temp = new List<IAstNode>();
            temp.AddRange(VariableDeclarations.Cast<IAstNode>());
            temp.AddRange(MethodDeclarations.Cast<IAstNode>());
            return temp;
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }

    public abstract class MemberDeclaration : IAstNode
    {
        public abstract List<IAstNode> Children();
        public abstract IAstNode CurrentNode();
    }

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

        public override string ToString()
        {
            return String.Join(",", MemberDeclarationsList);
        }
    }
    
    public class MethodDeclaration : MemberDeclaration
    {
        public readonly string MethodName;
        public readonly Parameters Parameters;
        public readonly string ReturnTypeName;
        public readonly StatementsBlock Body;


        public MethodDeclaration(
            Identifier methodName, 
            Parameters parameters, 
            Identifier returnType, 
            StatementsBlock body)
        {
            MethodName = methodName.name;
            Parameters = parameters;
            ReturnTypeName = returnType.name;
            Body = body;
        }

        public override string ToString()
        {
            return MethodName + "(" + Parameters + "):" + ReturnTypeName + "{" + Body + "}";
        }

        public override List<IAstNode> Children()
        {
            List<IAstNode> temp = new List<IAstNode>();
            temp.Add(Parameters);
            temp.Add(Body);
            return temp;
        }

        public override IAstNode CurrentNode()
        {
            return this;
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

        public override List<IAstNode> Children()
        {
            return new List<IAstNode>();
        }

        public override IAstNode CurrentNode()
        {
            return this;
        }
    }


    public class ConstructorDeclaration : MemberDeclaration
    {
        public readonly Parameters Parameters;
        public readonly StatementsBlock Body;

        public ConstructorDeclaration(Parameters parameters, StatementsBlock statementsBlock)
        {
            Parameters = parameters;
            Body = statementsBlock;
        }

        public override string ToString()
        {
            return "This(" + Parameters + ") {" + Body + "}";
        }

        public override List<IAstNode> Children()
        {
            List<IAstNode> temp = new List<IAstNode>();
            temp.Add(Parameters);
            temp.Add(Body);
            throw new NotImplementedException();
        }

        public override IAstNode CurrentNode()
        {
            throw new NotImplementedException();
        }
    }
    
    
    // A parameter (is not an expression)
    public class Parameter : IAstNode {
        public readonly string name;
        public readonly Identifier type;

        public Parameter(string name, Identifier type) {
            this.name = name;
            this.type = type;
        }

        public override string ToString()
        {
            return name + ": " + type;
        }

        public List<IAstNode> Children()
        {
            return new List<IAstNode>();
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }

    // A list of parameters (is not an expression)
    public class Parameters : IAstNode {
        public readonly List<Parameter> parames;

        public Parameters(List<Parameter> parames) {
            this.parames = parames;
        }

        public Parameters(Parameter parameter, Parameters parameters)
        {
            parames = new List<Parameter> { parameter };
            parames.AddRange(parameters.parames);

        }

        public Parameters(params Parameter[] parameters) : this(parameters.ToList()) {}
    

        public override string ToString()
        {
            return String.Join(",", parames);
        }

        public List<IAstNode> Children()
        {
            return parames.Cast<IAstNode>().ToList();
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }
}