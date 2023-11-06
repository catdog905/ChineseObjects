using System.Collections.Immutable;

namespace ChineseObjects.Lang;

// Base class for all statements
public interface Statement : IAstNode, IHumanReadable { }

// A combination of statements
public class StatementsBlock : Statement
{
    public ImmutableList<Statement> Stmts;

    public StatementsBlock(IEnumerable<Statement> stmts)
    {
        Stmts = stmts.ToImmutableList();
    }

    public StatementsBlock(
        StatementsBlock statementsBlock,
        Statement statement
    ) : this(statementsBlock.Stmts.Add(statement)) {}

    public StatementsBlock(
        Statement statement,
        StatementsBlock statementsBlock
    ) : this(new[] {statement}.Concat(statementsBlock.Stmts)) {}

    public StatementsBlock(params Statement[] statements) : this(statements.ToList()) { }

    public override string ToString()
    {
        return String.Join(";", Stmts);
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {};
        foreach(Statement stmt in Stmts)
        {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}
