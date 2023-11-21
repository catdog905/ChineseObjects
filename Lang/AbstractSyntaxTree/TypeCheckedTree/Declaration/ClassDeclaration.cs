using ChineseObjects.Lang.Declaration;

namespace ChineseObjects.Lang.TypeCheckedTree.Declaration;

public interface ITypeCheckedClassDeclaration : ITypeCheckedAstNode, ITypedAstNode
{
    public IEnumerable<Type> ParentClassNames();
    public IEnumerable<ITypeCheckedConstructorDeclaration> ConstructorDeclarations();
    public IEnumerable<ITypeCheckedVariableDeclaration> VariableDeclarations();
    public IEnumerable<ITypeCheckedMethodDeclaration> MethodDeclarations();
}

public class TypeCheckedClassDeclaration : ITypeCheckedClassDeclaration
{
    private readonly Type _type;
    private readonly IEnumerable<Type> _parentClassNames;
    private readonly IEnumerable<ITypeCheckedConstructorDeclaration> _constructorDeclarations;
    private readonly IEnumerable<ITypeCheckedVariableDeclaration> _variableDeclarations;
    private readonly IEnumerable<ITypeCheckedMethodDeclaration> _methodDeclarations;

    public TypeCheckedClassDeclaration(Type type, IEnumerable<Type> parentClassNames, IEnumerable<ITypeCheckedConstructorDeclaration> constructorDeclarations, IEnumerable<ITypeCheckedVariableDeclaration> variableDeclarations, IEnumerable<ITypeCheckedMethodDeclaration> methodDeclarations)
    {
        this._type = type;
        _parentClassNames = parentClassNames;
        _constructorDeclarations = constructorDeclarations;
        _variableDeclarations = variableDeclarations;
        _methodDeclarations = methodDeclarations;
        
        var fieldDuplicates = variableDeclarations
            .GroupBy(decl => decl.Name())
            .Where(group => group.Count() > 1);
        if (fieldDuplicates.Count() != 0)
            throw new DuplicateFieldsException(fieldDuplicates);
        
        var methodDuplicates = methodDeclarations
            .GroupBy(decl => decl.MethodName())
            .Where(group => group.Count() > 1);
        if (methodDuplicates.Count() != 0)
            throw new DuplicateMethodsException(methodDuplicates);
        //TODO : check type of statement block in every constructor
    }
    
    public TypeCheckedClassDeclaration(ITypedClass classDeclaration) :
        this(
            classDeclaration.Type(),
            classDeclaration.ParentClassNames(),
            classDeclaration.ConstructorDeclarations()
                .Select(decl => new TypeCheckedConstructorDeclaration(decl)),
            classDeclaration.VariableDeclarations()
                .Select(decl => new TypeCheckedVariableDeclaration(decl)),
            classDeclaration.MethodDeclarations()
                .Select(decl => new TypeCheckedMethodDeclaration(decl))) {}

    public Type Type()
    {
        return _type;
    }

    public IEnumerable<Type> ParentClassNames()
    {
        return _parentClassNames;
    }

    public IEnumerable<ITypeCheckedConstructorDeclaration> ConstructorDeclarations()
    {
        return _constructorDeclarations;
    }

    public IEnumerable<ITypeCheckedVariableDeclaration> VariableDeclarations()
    {
        return _variableDeclarations;
    }

    public IEnumerable<ITypeCheckedMethodDeclaration> MethodDeclarations()
    {
        return _methodDeclarations;
    }
}

public class DuplicateMethodsException : Exception
{
    public DuplicateMethodsException(IEnumerable<IGrouping<IIdentifier,ITypeCheckedMethodDeclaration>> methodDuplicates)
    {
        throw new NotImplementedException();
    }
}

public class DuplicateFieldsException : Exception
{
    public DuplicateFieldsException(IEnumerable<IGrouping<IIdentifier, ITypeCheckedVariableDeclaration>> fieldDuplicates)
    {
        throw new NotImplementedException();
    }
}
