using ChineseObjects.Lang.Declaration;

namespace ChineseObjects.Lang.TypeCheckedTree.Declaration;

public interface ITypeCheckedVariableDeclaration : ITypeCheckedAstNode, ITypedAstNode
{
    public IIdentifier Name();
}

public class TypeCheckedVariableDeclaration : ITypeCheckedVariableDeclaration
{
    public TypeCheckedVariableDeclaration(ITypedVariable variableDeclaration)
    {
        
    }

    public Type Type()
    {
        throw new NotImplementedException();
    }

    public IIdentifier Name()
    {
        throw new NotImplementedException();
    }
}