namespace ChineseObjects.Lang.Declaration;

public interface ITypesAwareProgram : IProgram, ITypesAwareAstNode
{
    public new IEnumerable<ITypesAwareClassDeclaration> ClassDeclarations();
}

class TypesAwareProgram : ITypesAwareProgram
{
    private readonly IEnumerable<ITypesAwareClassDeclaration> _classDeclarations;

    public TypesAwareProgram(IEnumerable<ITypesAwareClassDeclaration> classDeclarations)
    {
        _classDeclarations = classDeclarations;
    }
    
    public TypesAwareProgram(IEnumerable<IScopeAwareClassDeclaration> classDeclarations) :
        this(classDeclarations.Select(
            decl => new TypedAwareClassDeclaration(decl))) {}
    
    IEnumerable<IClassDeclaration> IProgram.ClassDeclarations()
    {
        return ClassDeclarations();
    }

    public IEnumerable<ITypesAwareClassDeclaration> ClassDeclarations()
    {
        return _classDeclarations;
    }
}