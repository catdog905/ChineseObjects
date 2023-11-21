using ChineseObjects.Lang.Declaration;
using ChineseObjects.Lang.TypeCheckedTree.Declaration.Parameter;
using ChineseObjects.Lang.TypeCheckedTree.Statement;

namespace ChineseObjects.Lang.TypeCheckedTree.Declaration;

public interface ITypeCheckedMethodDeclaration : ITypeCheckedAstNode
{
    public IIdentifier MethodName();
    public ITypeCheckedParameters Parameters();
    public Type ReturnType();
    public ITypeCheckedStatementsBlock Body();
}

public class TypeCheckedMethodDeclaration : ITypeCheckedMethodDeclaration
{
    public TypeCheckedMethodDeclaration(ITypesAwareMethod methodDeclaration)
    {
        
    }
    
    public IIdentifier MethodName()
    {
        throw new NotImplementedException();
    }

    public ITypeCheckedParameters Parameters()
    {
        throw new NotImplementedException();
    }

    public Type ReturnType()
    {
        throw new NotImplementedException();
    }

    public ITypeCheckedStatementsBlock Body()
    {
        throw new NotImplementedException();
    }
}