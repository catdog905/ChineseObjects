using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;
using TypedReference = System.TypedReference;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement;

// Base class for all statements
public interface IStatement : IHumanReadable, IAstNode {}

public interface IStatementsBlock : IAstNode, IHumanReadable
{
    public IEnumerable<IStatement> Statements();
}

public class StatementWrapper : IStatement
{
    private readonly IStatement _statement;

    public StatementWrapper(IStatement statement)
    {
        _statement = statement;
    }

    public StatementWrapper(ITypesAwareStatement statement) :
        this(statement switch
        {
            ITypesAwareAssignment assignment => new Assignment(assignment),
            ITypesAwareIfElse ifElse => new IfElse(ifElse),
            ITypesAwareReturn @return => new Return(@return),
            ITypesAwareWhile @while => new While(@while),
            ITypedExpression expression => new ExpressionWrapper(expression),
            _ => throw new NotImplementedException("Implementation of statement not found")
        }
        ) {}


    public IList<string> GetRepr()
    {
        return _statement.GetRepr();
    }
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

    public StatementsBlock(ITypesAwareStatementsBlock statementsBlock) :
        this(statementsBlock.Statements()
            .Select(statement => new StatementWrapper(statement))){}

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