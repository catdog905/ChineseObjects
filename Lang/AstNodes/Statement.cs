using System.Collections.Immutable;


namespace ChineseObjects.Lang
{

    // Base class for all statements
    public abstract class Statement
    {
    }

    // A return statement. Only stores the expression that is returned.
    public class Return : Statement
    {
        public readonly Expression retval;

        public Return(Expression retval)
        {
            this.retval = retval;
        }
    }

    // Assignment statement
    public class Assignment : Statement
    {
        public readonly string varname;
        public readonly Expression expr;

        public Assignment(string varname, Expression expr)
        {
            this.varname = varname;
            this.expr = expr;
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
    }

    // A combination of statements
    public class StatementsBlock : Statement
    {
        public ImmutableList<Statement> stmts;

        public StatementsBlock(ImmutableList<Statement> stmts)
        {
            this.stmts = stmts;
        }

        public StatementsBlock() : this(ImmutableList<Statement>.Empty)
        {
        }

        public StatementsBlock Append(Statement stmt)
        {
            return new StatementsBlock(stmts.Add(stmt));
        }
    }


    // A variable declaration (is not an expression)
    // TODO: should declarations of initialized and uninitialized variable
    // be the same or different types of nodes?
    public class VariableDeclaration : Statement
    {
        public readonly string name;
        public readonly Expression initializer;

        public VariableDeclaration(string name, Expression initializer)
        {
            this.name = name;
            this.initializer = initializer;
        }
    }
}
