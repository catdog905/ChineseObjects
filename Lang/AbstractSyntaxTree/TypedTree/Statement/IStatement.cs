namespace ChineseObjects.Lang;

public interface ITypesAwareStatement : ITypesAwareAstNode
{
    // `ITypesAwareStatementVisitor` is a general visitor interface to implement operations on `ITypesAwareStatement`s.
    // For instance, `LLVMCodeGen` compiles statements by visiting them.
    public T AcceptVisitor<T>(CodeGen.ITypesAwareStatementVisitor<T> visitor);
}

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
