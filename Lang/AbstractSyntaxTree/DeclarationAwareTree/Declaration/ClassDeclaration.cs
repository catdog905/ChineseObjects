using System.Collections.Immutable;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration;

public interface IClassDeclaration : IAstNode
{
    public IIdentifier ClassName();
    public ImmutableList<IIdentifier> ParentClassNames();
    public ImmutableList<IConstructorDeclaration> ConstructorDeclarations();
    public ImmutableList<IVariableDeclaration> VariableDeclarations();
    public ImmutableList<IMethodDeclaration> MethodDeclarations();
}

public class ClassDeclaration : IClassDeclaration, IHumanReadable
{
    private readonly IIdentifier _className;
    private readonly ImmutableList<IIdentifier> _parentClassNames;
    private readonly ImmutableList<IConstructorDeclaration> _constructorDeclarations;
    private readonly ImmutableList<IVariableDeclaration> _variableDeclarations;
    private readonly ImmutableList<IMethodDeclaration> _methodDeclarations;

    public ClassDeclaration(
        IIdentifier className, 
        ImmutableList<IIdentifier> parentClassNames, 
        ImmutableList<IConstructorDeclaration> constructorDeclarations, 
        ImmutableList<IVariableDeclaration> variableDeclarations, 
        ImmutableList<IMethodDeclaration> methodDeclarations)
    {
        _className = className;
        _parentClassNames = parentClassNames;
        _variableDeclarations = variableDeclarations;
        _methodDeclarations = methodDeclarations;
        
        if (constructorDeclarations.Count() == 0)
        {
            constructorDeclarations = constructorDeclarations.Add(
                new ConstructorDeclaration());
        }
        _constructorDeclarations = constructorDeclarations;
    }
    
    public ClassDeclaration(
        IIdentifier className, 
        IEnumerable<IIdentifier> parentClassNames, 
        IEnumerable<IConstructorDeclaration> constructorDeclarations, 
        IEnumerable<IVariableDeclaration> variableDeclarations, 
        IEnumerable<IMethodDeclaration> methodDeclarations)
        : this(
            className,
            parentClassNames.ToImmutableList(),
            constructorDeclarations.ToImmutableList(),
            variableDeclarations.ToImmutableList(),
            methodDeclarations.ToImmutableList()) {}

    public ClassDeclaration(
        IIdentifier className, 
        IIdentifiers parentClassNames, 
        IMemberDeclarations memberDeclarations)
        : this(
            className, 
            parentClassNames.GetIdentifiers(),
            ExtractConstructors(memberDeclarations),
            ExtractVariables(memberDeclarations),
            ExtractMethods(memberDeclarations)) {}

    private static IEnumerable<IConstructorDeclaration> ExtractConstructors(IMemberDeclarations memberDeclaration)
    {
        var constructors = memberDeclaration.GetMemberDeclarations()
            .OfType<IConstructorDeclaration>()
            .ToList();

        
        return constructors.ToImmutableList();
    }

    private static IEnumerable<IVariableDeclaration> ExtractVariables(IMemberDeclarations memberDeclaration)
    {
        var vars = memberDeclaration.GetMemberDeclarations()
            .OfType<IVariableDeclaration>()
            .ToList();
        
        return vars.ToImmutableList();
    }

    private static IEnumerable<IMethodDeclaration> ExtractMethods(IMemberDeclarations memberDeclaration)
    {
        var methods = memberDeclaration.GetMemberDeclarations()
            .OfType<IMethodDeclaration>()
            .ToList();
        
        return methods.ToImmutableList();
    }
    
    public ClassDeclaration(
        IIdentifier className, 
        IMemberDeclarations memberDeclarations
    ) : this(className, new Identifiers(), memberDeclarations) {}

    public ClassDeclaration(ITypesAwareClassDeclaration classDeclaration) : 
        this(new Identifier(classDeclaration.ClassName()),
            classDeclaration.ParentClassNames().
                Select(decl => decl.TypeName()),
            classDeclaration.ConstructorDeclarations()
                .Select(decl => new ConstructorDeclaration(decl)),
            classDeclaration.VariableDeclarations()
                .Select(decl => new VariableDeclaration(decl)),
            classDeclaration.MethodDeclarations()
                .Select(decl => new MethodDeclaration(decl)))
    {}

    public override string ToString()
    {
        return _className + "(vars="+String.Join(", ", _variableDeclarations) + "; methods=" + String.Join(", ", _methodDeclarations) + "), also constructors but we forgot them.";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string>{"CLASS " + _className};
        foreach(ConstructorDeclaration ctor in _constructorDeclarations)
        {
            ans.AddRange(ctor.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(VariableDeclaration var in _variableDeclarations)
        {
            ans.AddRange(var.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(MethodDeclaration method in _methodDeclarations)
        {
            ans.AddRange(method.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }

    public IIdentifier ClassName()
    {
        return _className;
    }

    public ImmutableList<IIdentifier> ParentClassNames()
    {
        return _parentClassNames;
    }

    public ImmutableList<IConstructorDeclaration> ConstructorDeclarations()
    {
        return _constructorDeclarations;
    }

    public ImmutableList<IVariableDeclaration> VariableDeclarations()
    {
        return _variableDeclarations;
    }

    public ImmutableList<IMethodDeclaration> MethodDeclarations()
    {
        return _methodDeclarations;
    }
    
    public static IClassDeclaration WithInheritedMethods(IClassDeclaration classDeclaration, Scope scope)
    {
        return new ClassDeclaration(
            classDeclaration.ClassName(),
            classDeclaration.ParentClassNames(),
            classDeclaration.ConstructorDeclarations(),
            classDeclaration.VariableDeclarations(),
            WithInheritedMethods(
                classDeclaration,
                ImmutableList<string>.Empty,
                scope).Values);
    }

    public static ImmutableDictionary<string, MethodDeclaration> WithInheritedMethods(
            IClassDeclaration classDeclaration,
            ImmutableList<string> childrenNames,
            Scope scope)
        {
            ImmutableList<string> parentNames = classDeclaration.ParentClassNames()
                .Select(parentClassName =>
                    scope.GetType(parentClassName.Value()).TypeName().Value())
                .ToImmutableList();
            if (parentNames.Distinct().Count() != parentNames.Count)
                throw new DuplicatedParentsException();
            if (childrenNames.Contains(classDeclaration.ClassName().Value()))
                throw new CyclicInheritanceException();
            
            ImmutableList<IClassDeclaration> parentClasses = classDeclaration.ParentClassNames()
                .Select(parentClassName =>
                    scope.GetType(parentClassName.Value()).ClassDeclaration())
                .ToImmutableList();
            
            ImmutableDictionary<string, MethodDeclaration> parentMethods = parentClasses.Aggregate(
                ImmutableDictionary<string, MethodDeclaration>.Empty,
                (dict, declaration) =>
                {
                    ImmutableDictionary<string, MethodDeclaration> parentMethods = WithInheritedMethods(
                        declaration, 
                        childrenNames.Add(classDeclaration.ClassName().Value()),
                        scope);
                    return dict
                        .RemoveRange(parentMethods.Keys)
                        .AddRange(WithInheritedMethods(
                            declaration, 
                            childrenNames.Add(classDeclaration.ClassName().Value()),
                            scope));
                });
            ImmutableDictionary<string, MethodDeclaration> currentMethods = classDeclaration.MethodDeclarations()
                .ToImmutableDictionary(
                    elem => elem.Signature(),
                    elem => (MethodDeclaration)elem);
            return parentMethods.RemoveRange(currentMethods.Keys).AddRange(currentMethods);
        }
}

public interface IThis : IExpression {}

public class This : IThis
{

    public override string ToString()
    {
        return "This";
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"THIS"};
    }
}

class CyclicInheritanceException : Exception { }
class DuplicatedParentsException : Exception { }
