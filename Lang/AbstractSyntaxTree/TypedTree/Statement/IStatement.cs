using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface ITypesAwareStatement : ITypesAwareAstNode {}

public interface ITypesAwareStatementsBlock : ITypesAwareAstNode
{
    public IEnumerable<ITypesAwareStatement> Statements();
}

public class TypesAwareStatementsBlock : ITypesAwareStatementsBlock{
    public TypesAwareStatementsBlock(IScopeAwareStatementsBlock statementsBlock) {}
    public IEnumerable<ITypesAwareStatement> Statements()
    {
        throw new NotImplementedException();
    }
}