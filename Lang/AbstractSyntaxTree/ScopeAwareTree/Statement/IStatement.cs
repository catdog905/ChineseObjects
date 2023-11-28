using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IScopeAwareStatement : IScopeAwareAstNode {}

public interface IScopeAwareStatementsBlock : IScopeAwareAstNode
{
    public IEnumerable<IScopeAwareStatement> Statements();
}

public class ScopeAwareStatementsBlock : IScopeAwareStatementsBlock
{
    private readonly Scope _scope;
    private readonly IEnumerable<IScopeAwareStatement> _stmts;

    public ScopeAwareStatementsBlock(Scope scope, IEnumerable<IStatement> stmts)
    {
        // Keep the original scope in the class, although more entities might be added to scope throughout the block 
        _scope = scope;

        Scope cur = scope;
        var awareStmts = new List<IScopeAwareStatement>();
        foreach (IStatement stmt in stmts)
        {
            IScopeAwareStatement aware = Irrealizable.MakeScopeAware(cur, stmt);
            if (stmt is Assignment assignment)
            {

                cur = new Scope(
                    cur, 
                    assignment.Name().Value(), 
                    new Type(scope, assignment.TypeName()));
            }
            awareStmts.Add(aware);
        }

        _stmts = awareStmts.ToImmutableList();
    }
    
    public ScopeAwareStatementsBlock(Scope scope, IStatementsBlock body) : this(scope, body.Statements()) {}

    public IEnumerable<IScopeAwareStatement> Statements()
    {
        return _stmts;
    }

    public Scope Scope()
    {
        return _scope;
    }
}
