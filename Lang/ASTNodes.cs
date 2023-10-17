using System.Collections.Immutable;

namespace ChineseObjects.Lang {
    // The base class for all expressions
    public abstract class Expression {}

    // The literal number expression (both for integers and floats)
    // TODO: separate integers and floats? Leave as is?
    public class NumLiteral : Expression {
        public readonly double value;

        public NumLiteral(double value) {
            this.value = value;
        }
    }

    // The boolean literal expression
    // TODO: merge with `NumLiteral`?
    public class BoolLiteral : Expression {
        public readonly bool value;

        public BoolLiteral(bool value) {
            this.value = value;
        }
    }

    // An identifier. Note that it is used to express that an `Identifier`
    // is an `Expression`. In more complex expressions that include identifiers
    // (such as variable/method/class declaration, etc) identifier is stored
    // as a mere `string` rather than the `Identifier` object.
    public class Identifier : Expression {
        public readonly string name;

        public Identifier(string name) {
            this.name = name;
        }
    }


    // Base class for all statements
    public abstract class Statement {}

    // A return statement. Only stores the expression that is returned.
    public class Return : Statement {
        public readonly Expression retval;

        public Return(Expression retval) {
            this.retval = retval;
        }
    }

    // Assignment statement
    public class Assignment : Statement {
        public readonly string varname;
        public readonly Expression expr;

        public Assignment(string varname, Expression expr) {
            this.varname = varname;
            this.expr = expr;
        }
    }

    // If-then[-else] statement
    public class IfElse : Statement {
        public readonly Expression cond;
        public readonly Statement then;
        public readonly Statement? else_;

        public IfElse(Expression cond, Statement then, Statement? else_) {
            this.cond = cond;
            this.then = then;
            this.else_ = else_;
        }
    }

    // While statement
    public class While : Statement {
        public readonly Expression cond;
        public readonly Statement body;

        public While(Expression cond, Statement body) {
            this.cond = cond;
            this.body = body;
        }
    }

    // A combination of statements
    public class StatementsBlock : Statement {
        public ImmutableList<Statement> stmts;

        public StatementsBlock(ImmutableList<Statement> stmts) {
            this.stmts = stmts;
        }

        public StatementsBlock() : this(ImmutableList<Statement>.Empty) {}

        public StatementsBlock Append(Statement stmt) {
            return new StatementsBlock(stmts.Add(stmt));
        }
    }


    // A variable declaration (is not an expression)
    // TODO: should declarations of initialized and uninitialized variable
    // be the same or different types of nodes?
    public class VariableDeclaration : Statement {
        public readonly string name;
        public readonly Expression initializer;

        public VariableDeclaration(string name, Expression initializer) {
            this.name = name;
            this.initializer = initializer;
        }
    }

    // A parameter (is not an expression)
    public class Parameter {
        public readonly string name;
        public readonly Expression type;

        public Parameter(string name, Expression type) {
            this.name = name;
            this.type = type;
        }
    }

    // A list of parameters (is not an expression)
    public class Parameters {
        public readonly ImmutableList<Parameter> parames;

        public Parameters(ImmutableList<Parameter> parames) {
            this.parames = parames;
        }

        public Parameters() : this(ImmutableList<Parameter>.Empty) {}

        public Parameters Append(Parameter param) {
            return new Parameters(parames.Add(param));
        }
    }

    // public class MethodDeclaration : Expression {
    //     public readonly string name;
    //     public readonly Parameters parames;
    //     public readonly ... body;
    // }
}
