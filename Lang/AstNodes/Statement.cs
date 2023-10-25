using System.Collections.Immutable;


namespace ChineseObjects.Lang
{

    // Base class for all statements
    public interface Statement : IAstNode, HumanReadable { }

    // A return statement. Only stores the expression that is returned.
    public class Return : Statement
    {
        public readonly Expression retval;

        public Return(Expression retval)
        {
            this.retval = retval;
        }

        public override string ToString()
        {
            return retval.ToString();
        }

        public IList<string> GetRepr()
        {
            var ans = new List<string> {"RETURN"};
            ans.AddRange(retval.GetRepr().Select(s => "| " + s));
            return ans;
        }
    }

    // Assignment statement
    public class Assignment : Statement
    {
        public readonly string Varname;
        public readonly Expression Expr;

        public Assignment(string varname, Expression expr)
        {
            Varname = varname;
            Expr = expr;
        }

        public override string ToString()
        {
            return Varname + ":=" + Expr;
        }

        public IList<string> GetRepr()
        {
            var ans = new List<string> {"ASSIGN TO " + Varname + ":"};
            ans.AddRange(Expr.GetRepr().Select(s => "| " + s));
            return ans;
        }
    }

    // If-then[-else] statement
    public class IfElse : Statement
    {
        public readonly Expression cond;
        public readonly Statement then;
        public readonly Statement? else_;

        public IfElse(Expression cond, Statement then, Statement? else_)
        {
            this.cond = cond;
            this.then = then;
            this.else_ = else_;
            
        }
        
        public IfElse(Expression cond, Statement then)
        {
            this.cond = cond;
            this.then = then;
        }

        public override string ToString()
        {
            return "IfElse(" + cond + "){" + then + "}{" + else_ + "}";
        }

        public IList<string> GetRepr()
        {
            var ans = new List<string> {"IF:"};
            ans.AddRange(cond.GetRepr().Select(s => "| " + s));
            ans.Add("THEN:");
            ans.AddRange(then.GetRepr().Select(s => "| " + s));
            if (else_ is not null)
            {
                ans.Add("ELSE:");
                ans.AddRange(else_.GetRepr().Select(s => "| " + s));
            }
            return ans;
        }
    }

    // While statement
    public class While : Statement
    {
        public readonly Expression cond;
        public readonly Statement body;

        public While(Expression cond, Statement body)
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
}
