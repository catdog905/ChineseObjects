using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public class Program : IAstNode, HumanReadable
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

public class ClassDeclaration : IAstNode, HumanReadable
{
    public readonly string ClassName;
    public readonly ImmutableList<string> ParentClassNames;
    public readonly ImmutableList<ConstructorDeclaration> ConstructorDeclarations;
    public readonly ImmutableList<VariableDeclaration> VariableDeclarations;
    public readonly ImmutableList<MethodDeclaration> MethodDeclarations;

    public ClassDeclaration(
        Identifier className, 
        Identifiers parentClassNames, 
        MemberDeclarations memberDeclarations)
    {
        ClassName = className.Name;
        ParentClassNames = parentClassNames.Names;
        var constructors = new List<ConstructorDeclaration> ();
        var vars = new List<VariableDeclaration> ();
        var methods = new List<MethodDeclaration> ();
        foreach (MemberDeclaration memberDeclaration in memberDeclarations.MemberDeclarations_)
        {
            switch (memberDeclaration)
            {
                case ConstructorDeclaration constructorDeclaration:
                    constructors.Add(constructorDeclaration);
                    break;
                case VariableDeclaration varDeclaration:
                    vars.Add(varDeclaration);
                    break;
                case MethodDeclaration methodDeclaration:
                    methods.Add(methodDeclaration);
                    break;
            }
        }
        ConstructorDeclarations = constructors.ToImmutableList();
        VariableDeclarations = vars.ToImmutableList();
        MethodDeclarations = methods.ToImmutableList();
    }
    
    public ClassDeclaration(
        Identifier className, 
        MemberDeclarations memberDeclarations
    ) : this(className, new Identifiers(), memberDeclarations) {}

    public override string ToString()
    {
        return ClassName + "(vars="+String.Join(", ", VariableDeclarations) + "; methods=" + String.Join(", ", MethodDeclarations) + "), also constructors but we forgot them.";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string>{"CLASS " + ClassName};
        foreach(ConstructorDeclaration ctor in ConstructorDeclarations)
        {
            ans.AddRange(ctor.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(VariableDeclaration var in VariableDeclarations)
        {
            ans.AddRange(var.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(MethodDeclaration method in MethodDeclarations)
        {
            ans.AddRange(method.GetRepr().Select(s => "| " + s));
        }
        return ans;
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

public class MethodDeclaration : MemberDeclaration, HumanReadable
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

    public IList<string> GetRepr()
    {
        var ans = new List<string>{"METHOD " + MethodName + ": " + ReturnTypeName};
        foreach(Parameter param in Parameters.Parames)
        {
            ans.AddRange(param.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(Statement stmt in Body.Stmts)
        {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}

// A variable declaration (is not an expression)
// TODO: should declarations of initialized and uninitialized variable
// be the same or different types of nodes?
public class VariableDeclaration : MemberDeclaration, HumanReadable
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

    public IList<string> GetRepr()
    {
        return new List<string>{"VARIABLE " + Name + ": " + Type};
    }
}


public class ConstructorDeclaration : MemberDeclaration, HumanReadable
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

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"CONSTRUCTOR"};
        foreach(Parameter param in Parameters.Parames)
        {
            ans.AddRange(param.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(Statement stmt in Body.Stmts) {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}


// A parameter (is not an expression)
public class Parameter : IAstNode, HumanReadable {
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

    public IList<string> GetRepr()
    {
        return new List<string> {"PARAMETER " + Name + ": " + Type};
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
