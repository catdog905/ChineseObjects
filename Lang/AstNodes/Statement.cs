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

        public List<IAstNode> Children()
        {
            return new List<IAstNode> { retval };
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }

    // Assignment statement
    public class Assignment : Statement
    {
        public readonly string varname;
        public readonly Object expr;

        public Assignment(string varname, Object expr)
        {
            this.varname = varname;
            this.expr = expr;
        }

        public override string ToString()
        {
            return varname + ":=" + expr;
        }

        public List<IAstNode> Children()
        {
            return new List<IAstNode> { expr };
        }

        public IAstNode CurrentNode()
        {
            return this;
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

        public List<IAstNode> Children()
        {
            if (else_ == null)
                return new List<IAstNode> {cond, then};
            else
                return new List<IAstNode> {cond, then, else_};
        }

        public IAstNode CurrentNode()
        {
            return this;
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

        public List<IAstNode> Children()
        {
            return new List<IAstNode> { cond, body };
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }

    // A combination of statements
    public class StatementsBlock : Statement
    {
        public List<Statement> stmts;

        public StatementsBlock(List<Statement> stmts)
        {
            this.stmts = stmts;
        }

        public StatementsBlock(Statement statement, StatementsBlock statementsBlock)
        {
            stmts = new List<Statement> { statement };
            stmts.AddRange(statementsBlock.stmts);
        }

        public StatementsBlock(params Statement[] statements) : this(statements.ToList()) { }

        public override string ToString()
        {
            return String.Join(";", stmts);
        }

        public List<IAstNode> Children()
        {
            return stmts.Cast<IAstNode>().ToList();
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }

    
}
