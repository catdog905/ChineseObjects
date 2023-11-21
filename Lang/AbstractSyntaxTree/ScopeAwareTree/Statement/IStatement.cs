using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareStatementsBlock : IScopeAwareAstNode
{
    public IEnumerable<IStatementDeclaration> Statements();
}

public class ScopeAwareStatementsBlock : IScopeAwareStatementsBlock {
    
    public ScopeAwareStatementsBlock(Scope scope, IDeclarationStatementsBlock body) {}

    public Scope Scope()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IStatementDeclaration> Statements()
    {
        throw new NotImplementedException();
    }
}