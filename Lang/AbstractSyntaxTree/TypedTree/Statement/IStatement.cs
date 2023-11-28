using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface ITypesAwareStatement : ITypesAwareAstNode {}

public interface ITypesAwareStatementsBlock : ITypesAwareAstNode
{
    public IEnumerable<ITypesAwareStatement> Statements();
}

public class TypesAwareStatementsBlock : ITypesAwareStatementsBlock
{
    private readonly IEnumerable<ITypesAwareStatement> _statements;

    public TypesAwareStatementsBlock(IEnumerable<ITypesAwareStatement> statements)
    {
        _statements = statements;
    }

    public TypesAwareStatementsBlock(IScopeAwareStatementsBlock statements) :
        this(statements.Statements()
            .Select(statement => TypeIrrealizable.MakeTypesAwareStatement(statement))
            .ToList()) {}

    public IEnumerable<ITypesAwareStatement> Statements()
    {
        return _statements;
    }
}