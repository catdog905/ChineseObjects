using System.Collections.Immutable;

namespace ChineseObjects.Lang {
    // The base class for all expressions
    public interface Expression : Statement, HumanReadable {}

    public class MethodCall : Expression, HumanReadable
    {
        public readonly Expression Caller;
        public readonly string MethodName;
        public readonly Arguments Arguments;

        public MethodCall(Expression caller, Identifier identifier, Arguments arguments)
        {
            Caller = caller;
            MethodName = identifier.Name;
            Arguments = arguments;
        }

        public override string ToString()
        {
            return "MethodCall(" + Caller + "." + MethodName + "(" + Arguments + "))";
        }

        public IList<string> GetRepr()
        {
            var ans = new List<string> {"CALL METHOD OF:"};
            ans.AddRange(Caller.GetRepr().Select(s => "| " + s));
            ans.Add("NAMED " + MethodName);
            ans.Add("WITH ARGS:");
            foreach(Argument arg in Arguments.Values)
            {
                ans.AddRange(arg.Value.GetRepr().Select(s => "| " + s));
            }
            return ans;
        }
    }

    public class Arguments
    {
        public readonly ImmutableList<Argument> Values;

        public Arguments(IEnumerable<Argument> values) {
            Values = values.ToImmutableList();
        }

        public Arguments(params Argument[] arguments) : this(arguments.ToImmutableList()) {}

        public Arguments(Arguments arguments, Argument argument) : this(arguments.Values.Add(argument)) {}

        public Arguments(Argument argument, Arguments arguments) : this(new[] {argument}.Concat(arguments.Values)) {}

        public override string ToString()
        {
            return String.Join(",", Values);
        }
    }

    public class Argument
    {
        public readonly Expression Value;

        public Argument(Expression value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
    
    // The literal number expression (both for integers and floats)
    // TODO: separate integers and floats? Leave as is?
    public class NumLiteral : Expression {
        public readonly double value;

        public NumLiteral(double value) {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public IList<string> GetRepr()
        {
            return new List<string> {"LITERAL " + value.ToString()};
        }
    }

    // The boolean literal expression
    // TODO: merge with `NumLiteral`?
    public class BoolLiteral : Expression {
        public readonly bool value;

        public BoolLiteral(bool value) {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public IList<string> GetRepr()
        {
            return new List<string> {"LITERAL " + value.ToString()};
        }
    }

    public class ClassInstantiation : Expression
    {
        public readonly string ClassName;
        public readonly Arguments Arguments;

        public ClassInstantiation(Identifier identifier, Arguments arguments)
        {
            ClassName = identifier.Name;
            Arguments = arguments;
        }

        public override string ToString()
        {
            return "new " + ClassName + "(" + Arguments + ")";
        }

        public IList<string> GetRepr()
        {
            var ans = new List<string> {"NEW " + ClassName};
            foreach(Argument arg in Arguments.Values)
            {
                ans.AddRange(arg.Value.GetRepr().Select(s => "| " + s));
            }
            return ans;
        }
    }

    public class This : Expression
    {
        public override string ToString()
        {
            return "This";
        }

        public IList<string> GetRepr()
        {
            return new List<string> {"THIS"};
        }
    }
    
}
