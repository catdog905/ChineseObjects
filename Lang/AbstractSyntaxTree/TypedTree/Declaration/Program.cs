namespace ChineseObjects.Lang.Declaration;

public interface ITypesAwareProgram : ITypesAwareAstNode
{
    public IEnumerable<ITypesAwareClassDeclaration> ClassDeclarations();
}

class TypesAwareProgram : ITypesAwareProgram
{
    private readonly IEnumerable<ITypesAwareClassDeclaration> _classDeclarations;

    public TypesAwareProgram(IEnumerable<ITypesAwareClassDeclaration> classDeclarations)
    {
        _classDeclarations = classDeclarations;
    }
    
    public TypesAwareProgram(IEnumerable<IScopeAwareClass> classDeclarations) :
        this(classDeclarations.Select(
            decl => new TypesAwareClassDeclaration(decl))
            .ToList()) {}
    
    public TypesAwareProgram(IScopeAwareProgram program) :
        this(program.ClassDeclarations()) {}

    public IEnumerable<ITypesAwareClassDeclaration> ClassDeclarations()
    {
        return _classDeclarations;
    }
}