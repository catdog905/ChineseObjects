namespace ChineseObjects.Lang.Declaration;

public interface ITypesAwareClassDeclaration : IClassDeclaration, ITypesAwareAstNode
{
    public new IEnumerable<Type> ParentClassNames();
    public new IEnumerable<ITypesAwareConstructorDeclaration> ConstructorDeclarations();
    public new IEnumerable<ITypesAwareVariableDeclaration> VariableDeclarations();
    public new IEnumerable<ITypesAwareMethodDeclaration> MethodDeclarations();
}

public class TypedAwareClassDeclaration : ITypesAwareClassDeclaration
{
    private readonly string _className;
    private readonly IEnumerable<Type> _parentClassNames;
    private readonly IEnumerable<ITypesAwareConstructorDeclaration> _constructorDeclarations;
    private readonly IEnumerable<ITypesAwareVariableDeclaration> _variableDeclarations;
    private readonly IEnumerable<ITypesAwareMethodDeclaration> _methodDeclarations;

    public TypedAwareClassDeclaration(
        string className, 
        IEnumerable<Type> parentClassNames, 
        IEnumerable<ITypesAwareConstructorDeclaration> constructorDeclarations, 
        IEnumerable<ITypesAwareVariableDeclaration> variableDeclarations, 
        IEnumerable<ITypesAwareMethodDeclaration> methodDeclarations)
    {
        _className = className;
        _parentClassNames = parentClassNames;
        _constructorDeclarations = constructorDeclarations;
        _variableDeclarations = variableDeclarations;
        _methodDeclarations = methodDeclarations;
    }

    public TypedAwareClassDeclaration(IScopeAwareClassDeclaration classDeclaration) :
        this(
            classDeclaration.ClassName(),
            classDeclaration.ParentClassNames()
                .Select(name => new Type(classDeclaration.Scope(), name)),
            classDeclaration.ConstructorDeclarations()
                .Select(decl => new TypesAwareConstructorDeclaration(decl)),
            classDeclaration.VariableDeclarations()
                .Select(decl => new TypesAwareVariableDeclaration(decl)),
            classDeclaration.MethodDeclarations()
                .Select(decl => new TypesAwareMethodDeclaration(decl))) {}

    public string ClassName()
    {
        return _className;
    }

    IEnumerable<Type> ITypesAwareClassDeclaration.ParentClassNames()
    {
        return _parentClassNames;
    }

    public IEnumerable<ITypesAwareConstructorDeclaration> ConstructorDeclarations()
    {
        return _constructorDeclarations;
    }

    public IEnumerable<ITypesAwareVariableDeclaration> VariableDeclarations()
    {
        return _variableDeclarations;
    }

    public IEnumerable<ITypesAwareMethodDeclaration> MethodDeclarations()
    {
        return _methodDeclarations;
    }

    IEnumerable<string> IClassDeclaration.ParentClassNames()
    {
        return _parentClassNames.Select(parent => parent.TypeName());
    }

    IEnumerable<IConstructorDeclaration> IClassDeclaration.ConstructorDeclarations()
    {
        return ConstructorDeclarations();
    }

    IEnumerable<IVariableDeclaration> IClassDeclaration.VariableDeclarations()
    {
        return VariableDeclarations();
    }

    IEnumerable<IMethodDeclaration> IClassDeclaration.MethodDeclarations()
    {
        return MethodDeclarations();
    }
}