namespace ChineseObjects.Lang;

public interface IScopeAwareStatement : IScopeAwareAstNode {}

public interface IScopeAwareStatementsBlock : IScopeAwareAstNode
{
    public IEnumerable<IScopeAwareStatement> Statements();
}

public class ScopeAwareStatementsBlock : IScopeAwareStatementsBlock {
    
    public ScopeAwareStatementsBlock(Scope scope, IStatementsBlock body) {}

    public IEnumerable<IScopeAwareStatement> Statements()
    {
        throw new NotImplementedException();
    }

    public Scope Scope()
    {
        throw new NotImplementedException();
    }
}
