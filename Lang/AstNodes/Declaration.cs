using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public class Program : IAstNode
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
}

public class ClassDeclaration : IAstNode
{
    public readonly string ClassName;
    public readonly ImmutableList<string> ParentClassNames;
    public readonly ImmutableList<VariableDeclaration> VariableDeclarations;
    public readonly ImmutableList<MethodDeclaration> MethodDeclarations;
    public readonly ImmutableList<ConstructorDeclaration> ConstructorDeclarations;

    public ClassDeclaration(
        Identifier className, 
        Identifiers parentClassNames, 
        MemberDeclarations memberDeclarations)
    {
        ClassName = className.Name;
        ParentClassNames = parentClassNames.Names;
        var vars = new List<VariableDeclaration> ();
        var methods = new List<MethodDeclaration> ();
        var constructors = new List<ConstructorDeclaration> ();
        foreach (MemberDeclaration memberDeclaration in memberDeclarations.MemberDeclarations_)
        {
            switch (memberDeclaration)
            {
                case VariableDeclaration varDeclaration:
                    vars.Add(varDeclaration);
                    break;
                case MethodDeclaration methodDeclaration:
                    methods.Add(methodDeclaration);
                    break;
                case ConstructorDeclaration constructorDeclaration:
                    constructors.Add(constructorDeclaration);
                    break;
            }
        }
        VariableDeclarations = vars.ToImmutableList();
        MethodDeclarations = methods.ToImmutableList();
        ConstructorDeclarations = constructors.ToImmutableList();
    }
    
    public ClassDeclaration(
        Identifier className, 
        MemberDeclarations memberDeclarations
    ) : this(className, new Identifiers(), memberDeclarations) {}

    public override string ToString()
    {
        return ClassName + "(vars="+String.Join(", ", VariableDeclarations) + "; methods=" + String.Join(", ", MethodDeclarations) + ")";
    }
}

public abstract class MemberDeclaration : IAstNode {}

public class MemberDeclarations
{
    public readonly ImmutableList<MemberDeclaration> MemberDeclarations_;

    public MemberDeclarations(IEnumerable<MemberDeclaration> members) {
        MemberDeclarations_ = members.ToImmutableList();
    }

    public MemberDeclarations() : this(ImmutableList<MemberDeclaration>.Empty) {}
    
    public MemberDeclarations(
        MemberDeclarations memberDeclarations,
        MemberDeclaration memberDeclaration
    ) : this(memberDeclarations.MemberDeclarations_.Add(memberDeclaration)) {}

    public MemberDeclarations(
        MemberDeclaration memberDeclaration,
        MemberDeclarations memberDeclarations
    ) : this(new[] {memberDeclaration}.Concat(memberDeclarations.MemberDeclarations_)) {}

    public override string ToString()
    {
        return String.Join(",", MemberDeclarations_);
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
        MethodName = methodName.Name;
        Parameters = parameters;
        ReturnTypeName = returnType.Name;
        Body = body;
    }

    public override string ToString()
    {
        return MethodName + "(" + Parameters + "):" + ReturnTypeName + "{" + Body + "}";
    }
}

// A variable declaration (is not an expression)
// TODO: should declarations of initialized and uninitialized variable
// be the same or different types of nodes?
public class VariableDeclaration : MemberDeclaration
{
    public readonly string Name;
    public readonly string Type;

    public VariableDeclaration(Identifier name, Identifier type)
    {
        Name = name.Name;
        Type = type.Name;
    }

    public override string ToString()
    {
        return Name + ":" + Type;
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
}


// A parameter (is not an expression)
public class Parameter : IAstNode {
    public readonly string Name;
    public readonly string Type;

    public Parameter(string name, Identifier type) {
        Name = name;
        Type = type.Name;
    }

    public override string ToString()
    {
        return Name + ": " + Type;
    }
}

// A list of parameters (is not an expression)
public class Parameters : IAstNode {
    public readonly ImmutableList<Parameter> Parames;

    public Parameters(IEnumerable<Parameter> parames)
    {
        Parames = parames.ToImmutableList();
    }

    public Parameters(
        Parameters parameters,
        Parameter parameter
    ) : this(parameters.Parames.Add(parameter)) {}

    public Parameters(
        Parameter parameter,
        Parameters parameters
    ) : this(new[] {parameter}.Concat(parameters.Parames)) {}

    public Parameters(params Parameter[] parameters) : this(parameters.ToList()) {}


    public override string ToString()
    {
        return String.Join(",", Parames);
    }
}
