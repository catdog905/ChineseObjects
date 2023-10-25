using System.Collections.Immutable;


namespace ChineseObjects.Lang
{

    // Base class for all statements
    public interface Statement : IAstNode { }

    // A return statement. Only stores the expression that is returned.
    public class Return : Statement
    {
        public readonly Object retval;

        public Return(Object retval)
        {
            this.retval = retval;
        }

        public override string ToString()
        {
            return retval.ToString();
        }
    }

    // Assignment statement
    public class Assignment : Statement
    {
        public readonly string Varname;
        public readonly Expression Expr;

        public Assignment(string varname, Object expr)
        {
            Varname = varname;
            Expr = expr;
        }

        public override string ToString()
        {
            return Varname + ":=" + Expr;
        }
    }

    // If-then[-else] statement
    public class IfElse : Statement
    {
        public readonly Object cond;
        public readonly Statement then;
        public readonly Statement? else_;

        public IfElse(Object cond, Statement then, Statement? else_)
        {
            this.cond = cond;
            this.then = then;
            this.else_ = else_;
            
        }
        
        public IfElse(Object cond, Statement then)
        {
            this.cond = cond;
            this.then = then;
        }

        public override string ToString()
        {
            return "IfElse(" + cond + "){" + then + "}{" + else_ + "}";
        }
    }

    // While statement
    public class While : Statement
    {
        public readonly Object cond;
        public readonly Statement body;

        public While(Object cond, Statement body)
        {
            this.cond = cond;
            this.body = body;
        }

        public override string ToString()
        {
            return "While(" + cond + "){" + body + "}";
        }

        public IList<string> GetRepr()
        {
            var ans = new List<string> {"WHILE:"};
            ans.AddRange(cond.GetRepr().Select(s => "| " + s));
            ans.Add("DO:");
            ans.AddRange(body.GetRepr().Select(s => "| " + s));
            return ans;
        }
    }

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
    }
}
