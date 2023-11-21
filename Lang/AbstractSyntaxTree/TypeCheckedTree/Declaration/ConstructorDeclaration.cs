using ChineseObjects.Lang.Declaration;
using ChineseObjects.Lang.TypeCheckedTree.Declaration.Parameter;
using ChineseObjects.Lang.TypeCheckedTree.Statement;

namespace ChineseObjects.Lang.TypeCheckedTree.Declaration;

public interface ITypeCheckedConstructorDeclaration : ITypeCheckedAstNode
{
    public ITypeCheckedParameters Parameters();
    public ITypeCheckedStatementsBlock Body();
}

public class TypeCheckedConstructorDeclaration : ITypeCheckedConstructorDeclaration {

    public TypeCheckedConstructorDeclaration(ITypesAwareConstructor constructorDeclaration)
    {
        throw new NotImplementedException();
    }

    public ITypeCheckedParameters Parameters()
    {
        throw new NotImplementedException();
    }

    public ITypeCheckedStatementsBlock Body()
    {
        throw new NotImplementedException();
    }
}