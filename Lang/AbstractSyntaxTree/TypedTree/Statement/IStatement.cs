using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface ITypesAwareStatementsBlock : IStatementsBlock, ITypesAwareAstNode
{
    public new IEnumerable<IStatementDeclaration> Statements();
}

public class TypesAwareStatementsBlock : ITypesAwareStatementsBlock{
    public TypesAwareStatementsBlock(IScopeAwareStatementsBlock statementsBlock) {}
    
    IEnumerable<IStatement> IStatementsBlock.Statements()
    {
        return Statements();
    }

    public IEnumerable<IStatementDeclaration> Statements()
    {
        throw new NotImplementedException();
    }
}