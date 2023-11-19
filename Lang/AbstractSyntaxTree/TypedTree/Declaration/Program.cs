namespace ChineseObjects.Lang.Declaration;

public interface ITypesAwareProgram : IProgram, ITypesAwareAstNode
{
    public IEnumerable<ITypedClass> ClassDeclarations();
}

class TypesAwareProgram : ITypesAwareProgram
{
    private readonly IEnumerable<ITypedClass> _classDeclarations;

    public TypesAwareProgram(IEnumerable<ITypedClass> classDeclarations)
    {
        _classDeclarations = classDeclarations;
    }
    
    public TypesAwareProgram(IEnumerable<IScopeAwareClass> classDeclarations) :
        this(classDeclarations.Select(
            decl => new TypedClass(decl))) {}

    public IEnumerable<ITypedClass> ClassDeclarations()
    {
        return _classDeclarations;
    }
}