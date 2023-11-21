using System.Collections.Immutable;

namespace ChineseObjects.Lang;

// Base class for all statements
public interface IStatementDeclaration : IHumanReadable, IDeclarationAstNode {}

public interface IDeclarationStatementsBlock : IDeclarationAstNode
{
    public IEnumerable<IStatementDeclaration> Statements();
}

// A combination of statements
public class StatementsBlock : IDeclarationStatementsBlock
{
    public ImmutableList<IStatementDeclaration> _statements;

    public StatementsBlock(IEnumerable<IStatementDeclaration> stmts)
    {
        _statements = stmts.ToImmutableList();
    }

    public StatementsBlock(
        StatementsBlock statementsBlock,
        IStatementDeclaration statementDeclaration
    ) : this(statementsBlock._statements.Add(statementDeclaration)) {}

    public StatementsBlock(
        IStatementDeclaration statementDeclaration,
        StatementsBlock statementsBlock
    ) : this(new[] {statementDeclaration}.Concat(statementsBlock._statements)) {}

    public StatementsBlock(params IStatementDeclaration[] statements) : this(statements.ToList()) { }

    public override string ToString()
    {
        return String.Join(";", _statements);
    }

    public IEnumerable<IStatementDeclaration> Statements()
    {
        return _statements;
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {};
        foreach(IStatementDeclaration stmt in _statements)
        {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}