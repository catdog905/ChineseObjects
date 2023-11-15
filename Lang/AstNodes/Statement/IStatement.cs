using System.Collections.Immutable;

namespace ChineseObjects.Lang;

// Base class for all statements
public interface IStatement : IAstNode, IHumanReadable {}

public interface IStatementsBlock : IStatement
{
    public IEnumerable<IStatement> Statements();
}

public interface IScopeAwareStatementsBlock : IStatementsBlock, IScopeAwareAstNode
{
    new IEnumerable<IStatement> Statements();
}

// A combination of statements
public class StatementsBlock : IStatementsBlock
{
    public ImmutableList<IStatement> _statements;

    public StatementsBlock(IEnumerable<IStatement> stmts)
    {
        _statements = stmts.ToImmutableList();
    }

    public StatementsBlock(
        StatementsBlock statementsBlock,
        IStatement statement
    ) : this(statementsBlock._statements.Add(statement)) {}

    public StatementsBlock(
        IStatement statement,
        StatementsBlock statementsBlock
    ) : this(new[] {statement}.Concat(statementsBlock._statements)) {}

    public StatementsBlock(params IStatement[] statements) : this(statements.ToList()) { }

    public override string ToString()
    {
        return String.Join(";", _statements);
    }

    public IEnumerable<IStatement> Statements()
    {
        return _statements;
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {};
        foreach(IStatement stmt in _statements)
        {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }
}


public class ScopeAwareStatementsBlock : IScopeAwareStatementsBlock{
    public ScopeAwareStatementsBlock(Scope scope, StatementsBlock body)
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