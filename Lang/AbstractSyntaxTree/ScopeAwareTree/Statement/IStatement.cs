using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareStatementsBlock : IScopeAwareAstNode
{
    public IEnumerable<IStatement> Statements();
}

public class ScopeAwareStatementsBlock : IScopeAwareStatementsBlock {
    
    public ScopeAwareStatementsBlock(Scope scope, IStatementsBlock body) {}

    public Scope Scope()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IStatement> Statements()
    {
        throw new NotImplementedException();
    }
}