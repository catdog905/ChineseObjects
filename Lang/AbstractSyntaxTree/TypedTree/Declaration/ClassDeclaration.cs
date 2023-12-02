using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Declaration;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;

public interface ITypesAwareClassDeclaration : ITypesAwareAstNode
{
    public string ClassName();
    public IEnumerable<Type> ParentClassNames();
    public IEnumerable<ITypesAwareConstructor> ConstructorDeclarations();
    public IEnumerable<ITypedVariable> VariableDeclarations();
    public IEnumerable<ITypesAwareMethod> MethodDeclarations();
}

public class TypesAwareClassDeclaration : ITypesAwareClassDeclaration
{
    private readonly string _className;
    private readonly IEnumerable<Type> _parentClassNames;
    private readonly IEnumerable<ITypesAwareConstructor> _constructorDeclarations;
    private readonly IEnumerable<ITypedVariable> _variableDeclarations;
    private readonly IEnumerable<ITypesAwareMethod> _methodDeclarations;

    public TypesAwareClassDeclaration(
        string className,
        IEnumerable<Type> parentClassNames, 
        IEnumerable<ITypesAwareConstructor> constructorDeclarations, 
        IEnumerable<ITypedVariable> variableDeclarations, 
        IEnumerable<ITypesAwareMethod> methodDeclarations)
    {
        _className = className;
        _parentClassNames = parentClassNames;
        _constructorDeclarations = constructorDeclarations;
        _variableDeclarations = variableDeclarations;
        _methodDeclarations = methodDeclarations;

        if (constructorDeclarations
                .GroupBy(decl => decl.Parameters())
                .Count(group => group.Count() > 1) != 0)
            throw new DuplicatedConstructorException(_className);
        if (methodDeclarations
                .GroupBy(decl => (decl.MethodName(), decl.Parameters()))
                .Count(group => group.Count() > 1) != 0)
            throw new DuplicatedMethodException(_className);
    }

    public TypesAwareClassDeclaration(IScopeAwareClass scopeAwareClass) :
        this(
            scopeAwareClass.ClassName().Value(),
            scopeAwareClass.ParentClassNames()
                .Select(parentClassName => new Type(scopeAwareClass.Scope(), parentClassName))
                .ToList(),
            scopeAwareClass.ConstructorDeclarations()
                .Select(decl => new TypesAwareConstructor(decl))
                .ToList(),
            scopeAwareClass.VariableDeclarations()
                .Select(decl => new TypedVariable(decl))
                .ToList(),
            scopeAwareClass.MethodDeclarations()
                .Select(decl => new TypesAwareMethod(decl))
                .ToList()) {}


    public string ClassName()
    {
        return _className;
    }

    public IEnumerable<Type> ParentClassNames()
    {
        return _parentClassNames;
    }

    public IEnumerable<ITypesAwareConstructor> ConstructorDeclarations()
    {
        return _constructorDeclarations;
    }

    public IEnumerable<ITypedVariable> VariableDeclarations()
    {
        return _variableDeclarations;
    }

    public IEnumerable<ITypesAwareMethod> MethodDeclarations()
    {
        return _methodDeclarations;
    }
    
    
}

public class DuplicatedConstructorException : Exception
{
    public DuplicatedConstructorException(string className) : 
        base(className + " have two constructors with the same signature") { }
}

public class DuplicatedMethodException : Exception
{
    public DuplicatedMethodException(string className) : 
        base(className + " have two method with the same signature") { }
}