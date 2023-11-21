namespace ChineseObjects.Lang.Declaration;

public interface ITypedClass : ITypedAstNode
{
    public IEnumerable<Type> ParentClassNames();
    public IEnumerable<ITypesAwareConstructor> ConstructorDeclarations();
    public IEnumerable<ITypedVariable> VariableDeclarations();
    public IEnumerable<ITypesAwareMethod> MethodDeclarations();
}

public class TypedClass : ITypedClass
{
    private readonly Type _type;
    private readonly IEnumerable<Type> _parentClassNames;
    private readonly IEnumerable<ITypesAwareConstructor> _constructorDeclarations;
    private readonly IEnumerable<ITypedVariable> _variableDeclarations;
    private readonly IEnumerable<ITypesAwareMethod> _methodDeclarations;

    public TypedClass(
        Type type, 
        IEnumerable<Type> parentClassNames, 
        IEnumerable<ITypesAwareConstructor> constructorDeclarations, 
        IEnumerable<ITypedVariable> variableDeclarations, 
        IEnumerable<ITypesAwareMethod> methodDeclarations)
    {
        _type = type;
        _parentClassNames = parentClassNames;
        _constructorDeclarations = constructorDeclarations;
        _variableDeclarations = variableDeclarations;
        _methodDeclarations = methodDeclarations;
    }

    public TypedClass(IScopeAwareClass scopeAwareClass) :
        this(
            new Type(scopeAwareClass.Scope(), scopeAwareClass.ClassName().Name()),
            scopeAwareClass.ParentClassNames()
                .Select(name => new Type(scopeAwareClass.Scope(), name)),
            scopeAwareClass.ConstructorDeclarations()
                .Select(decl => new TypesAwareConstructor(decl)),
            scopeAwareClass.VariableDeclarations()
                .Select(decl => new TypedVariable(decl)),
            scopeAwareClass.MethodDeclarations()
                .Select(decl => new TypesAwareMethod(decl))) {}


    public Type Type()
    {
        return _type;
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