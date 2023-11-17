using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareStatementsBlock : IStatementsBlock, IScopeAwareAstNode
{
    public new IEnumerable<IStatement> Statements();
}

public class ScopeAwareStatementsBlock : IScopeAwareStatementsBlock{
    public ScopeAwareStatementsBlock(Scope scope, IStatementsBlock body)
    {
        throw new NotImplementedException();
    }


    IEnumerable<IStatement> IStatementsBlock.Statements()
    {
        throw new NotImplementedException();
    }

    public Scope Scope()
    {
        throw new NotImplementedException();
    }

    IEnumerable<IStatement> IScopeAwareStatementsBlock.Statements()
    {
        throw new NotImplementedException();
    }

    public IList<string> GetRepr()
    {
        throw new NotImplementedException();
    }
}