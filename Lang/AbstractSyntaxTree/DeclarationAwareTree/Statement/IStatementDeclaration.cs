using System.Collections.Immutable;

namespace ChineseObjects.Lang;

// Base class for all statements
public interface IStatementDeclaration : IStatement, IHumanReadable, IDeclarationAstNode {}

public interface IDeclarationStatementsBlock : IStatementsBlock, IDeclarationAstNode
{
    public new IEnumerable<IStatementDeclaration> Statements();
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

    IEnumerable<IStatement> IStatementsBlock.Statements()
    {
        return Statements();
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