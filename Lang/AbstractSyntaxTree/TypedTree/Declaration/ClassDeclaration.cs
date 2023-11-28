namespace ChineseObjects.Lang.Declaration;

public interface ITypesAwareClassDeclaration : ITypesAwareAstNode
{
    public IEnumerable<Type> ParentClassNames();
    public IEnumerable<ITypesAwareConstructor> ConstructorDeclarations();
    public IEnumerable<ITypedVariable> VariableDeclarations();
    public IEnumerable<ITypesAwareMethod> MethodDeclarations();
}

public class TypesAwareClassDeclaration : ITypesAwareClassDeclaration
{
    private readonly IEnumerable<Type> _parentClassNames;
    private readonly IEnumerable<ITypesAwareConstructor> _constructorDeclarations;
    private readonly IEnumerable<ITypedVariable> _variableDeclarations;
    private readonly IEnumerable<ITypesAwareMethod> _methodDeclarations;

    public TypesAwareClassDeclaration(
        IEnumerable<Type> parentClassNames, 
        IEnumerable<ITypesAwareConstructor> constructorDeclarations, 
        IEnumerable<ITypedVariable> variableDeclarations, 
        IEnumerable<ITypesAwareMethod> methodDeclarations)
    {
        _parentClassNames = parentClassNames;
        _constructorDeclarations = constructorDeclarations;
        _variableDeclarations = variableDeclarations;
        _methodDeclarations = methodDeclarations;
    }

    public TypesAwareClassDeclaration(IScopeAwareClass scopeAwareClass) :
        this(
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