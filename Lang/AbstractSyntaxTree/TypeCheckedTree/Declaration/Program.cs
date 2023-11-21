using ChineseObjects.Lang.Declaration;

namespace ChineseObjects.Lang.TypeCheckedTree.Declaration;

public interface ITypeCheckedProgram : ITypeCheckedAstNode
{
    public IEnumerable<ITypeCheckedClassDeclaration> ClassDeclarations();
}

public class TypeCheckedProgram : ITypeCheckedProgram
{
    private readonly IEnumerable<ITypeCheckedClassDeclaration> _classDeclarations;

    public TypeCheckedProgram(IEnumerable<ITypeCheckedClassDeclaration> classDeclarations)
    {
        _classDeclarations = classDeclarations;
        var duplicates = classDeclarations
            .GroupBy(decl => decl.Type())
            .Where(group => group.Count() > 1);
        if (duplicates.Count() != 0)
            throw new DuplicateClassException(duplicates);
    }
    
    public TypeCheckedProgram(ITypesAwareProgram program) :
        this(program.ClassDeclarations()
            .Select(decl => new TypeCheckedClassDeclaration(decl))) {}
    
    public IEnumerable<ITypeCheckedClassDeclaration> ClassDeclarations()
    {
        return _classDeclarations;
    }
}

public class DuplicateClassException : Exception
{
    public DuplicateClassException(IEnumerable<IGrouping<Type, ITypeCheckedClassDeclaration>> where)
    {
        throw new NotImplementedException();
    }

    public DuplicateClassException(IEnumerable<IGrouping<IIdentifier,ITypeCheckedMethodDeclaration>> methodDuplicates)
    {
        throw new NotImplementedException();
    }
    //TODO : finish implementation
}