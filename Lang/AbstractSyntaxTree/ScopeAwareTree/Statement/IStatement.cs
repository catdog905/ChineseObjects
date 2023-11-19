using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareStatementsBlock : IStatementsBlock, IScopeAwareAstNode
{
    public new IEnumerable<IStatementDeclaration> Statements();
}

public class ScopeAwareStatementsBlock : IScopeAwareStatementsBlock {
    
    public ScopeAwareStatementsBlock(Scope scope, IDeclarationStatementsBlock body) {}
    
    IEnumerable<IStatement> IStatementsBlock.Statements()
    {
        return Statements();
    }

    public Scope Scope()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IStatementDeclaration> Statements()
    {
        throw new NotImplementedException();
    }
}