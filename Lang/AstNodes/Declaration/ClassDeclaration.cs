using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public class ClassDeclaration : IAstNode, IHumanReadable
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