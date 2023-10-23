namespace ChineseObjects.Lang {
    // The base class for all expressions
    public interface Object : IAstNode {}

    public class MethodCall : Object, Statement
    {
        public readonly Object Caller;
        public readonly string MethodName;
        public readonly Arguments Arguments;

        public MethodCall(Object caller, Identifier identifier, Arguments arguments)
        {
            Caller = caller;
            MethodName = identifier.name;
            Arguments = arguments;
        }

        public override string ToString()
        {
            return "MethodCall(" + Caller + "." + MethodName + "(" + Arguments + "))";
        }

        public List<IAstNode> Children()
        {
            return new List<IAstNode> { Caller, Arguments };
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }

    public class Arguments : Object
    {
        public readonly List<Argument> Values;

        public Arguments(params Argument[] arguments)
        {
            Values = arguments.ToList();
        } 
        
        public Arguments(Argument argument)
        {
            Values = new List<Argument> { argument };
        }

        public Arguments(Argument argument, Arguments arguments)
        {
            Values = new List<Argument> { argument };
            Values.AddRange(arguments.Values);
        }

        public override string ToString()
        {
            return String.Join(",", Values);
        }

        public List<IAstNode> Children()
        {
            return Values.Cast<IAstNode>().ToList();
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }

    public class Argument : Object
    {
        public readonly Object Value;

        public Argument(Object value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public List<IAstNode> Children()
        {
            return new List<IAstNode> { Value };
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }
    
    // The literal number expression (both for integers and floats)
    // TODO: separate integers and floats? Leave as is?
    public class NumLiteral : Object {
        public readonly double value;

        public NumLiteral(double value) {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public List<IAstNode> Children()
        {
            return new List<IAstNode>();
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }

    // The boolean literal expression
    // TODO: merge with `NumLiteral`?
    public class BoolLiteral : Object {
        public readonly bool value;

        public BoolLiteral(bool value) {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public List<IAstNode> Children()
        {
            return new List<IAstNode>();
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }

    public class ClassInstantiation : Object
    {
        public readonly string ClassName;
        public readonly Arguments Arguments;

        public ClassInstantiation(Identifier identifier, Arguments arguments)
        {
            ClassName = identifier.name;
            Arguments = arguments;
        }

        public override string ToString()
        {
            return "new " + ClassName + "(" + Arguments + ")";
        }

        public List<IAstNode> Children()
        {
            return new List<IAstNode> { Arguments };
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }

    public class This : Object
    {
        public override string ToString()
        {
            return "This";
        }

        public List<IAstNode> Children()
        {
            return new List<IAstNode>();
        }

        public IAstNode CurrentNode()
        {
            return this;
        }
    }
    
}
