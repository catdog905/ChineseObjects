namespace ChineseObjects.Lang.TypeCheckedTree.Statement;

public interface ITypeCheckedStatement : ITypeCheckedAstNode {}

public interface ITypeCheckedStatementsBlock : ITypeCheckedAstNode
{
    public IEnumerable<ITypeCheckedStatement> Statements();
}

public class TypeCheckedStatementsBlock : ITypeCheckedStatementsBlock{
    public TypeCheckedStatementsBlock(IScopeAwareStatementsBlock statementsBlock) {}
    
    public IEnumerable<ITypeCheckedStatement> Statements()
    {
        throw new NotImplementedException();
    }
}