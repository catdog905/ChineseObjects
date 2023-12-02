using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement;

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