using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

public interface ITypedClassInstantiation : ITypedExpression
{
    public string ClassName();
    public ITypesAwareArguments Arguments();
}

public class TypedClassInstantiation : ITypedClassInstantiation
{
    private readonly Type _type;
    private readonly string _className;
    private readonly ITypesAwareArguments _arguments;

    public TypedClassInstantiation(
        Type type, 
        string className, 
        ITypesAwareArguments arguments)
    {
        _type = type;
        _className = className;
        _arguments = arguments;

        _type.ConstructorCallCheck(arguments);
    }

    public TypedClassInstantiation(IScopeAwareClassInstantiation classInstantiation) :
        this(
            new Type(classInstantiation.Scope(), 
                classInstantiation.ClassName()), 
            classInstantiation.ClassName().Value(),
            new TypesAwareArguments(classInstantiation.Arguments())) {}
    
    // public TypedMethodCall(IScopeAwareMethodCall methodCall) :
    //     this(
    //         TypeIrrealizable.MakeTypedExpression(methodCall.Caller())
    //             .Type().MethodCallReturnType(methodCall),
    //         TypeIrrealizable.MakeTypedExpression(methodCall.Caller()),
    //         methodCall.MethodName().Value(),
    //         new TypesAwareArguments(methodCall.Arguments())){}


    public Type Type()
    {
        return _type;
    }

    public T AcceptVisitor<T>(CodeGen.ITypesAwareStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public string ClassName()
    {
        return _className;
    }

    public ITypesAwareArguments Arguments()
    {
        return _arguments;
    }

    public IList<string> GetRepr()
    {
        return new DeclarationAwareTree.Statement.Expression.ClassInstantiation(this).GetRepr();
    }
}