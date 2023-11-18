using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface ITypesAwareStatementsBlock : IStatementsBlock, ITypesAwareAstNode
{
    public new IEnumerable<IStatement> Statements();
}

public class TypesAwareStatementsBlock : ITypesAwareStatementsBlock{
    public TypesAwareStatementsBlock(IScopeAwareStatementsBlock statementsBlock)
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

    IEnumerable<IStatement> ITypesAwareStatementsBlock.Statements()
    {
        throw new NotImplementedException();
    }

    public IList<string> GetRepr()
    {
        throw new NotImplementedException();
    }
}