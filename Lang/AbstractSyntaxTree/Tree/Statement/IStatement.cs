namespace ChineseObjects.Lang;

// Base class for all statements
public interface IStatement : IAstNode {}

public interface IStatementsBlock : IStatement
{
    public IEnumerable<IStatement> Statements();
}