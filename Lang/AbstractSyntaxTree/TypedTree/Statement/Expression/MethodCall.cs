using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

public interface ITypedMethodCall : ITypedExpression
{
    public ITypedExpression Caller();
    public string MethodName();
    public ITypesAwareArguments Arguments(); 
}

public class TypedMethodCall : ITypedMethodCall
{
    private readonly Type _type;
    private readonly ITypedExpression _caller;
    private readonly string _methodName;
    private readonly ITypesAwareArguments _arguments;

    public TypedMethodCall(
        Type type, 
        ITypedExpression caller, 
        string methodName, 
        ITypesAwareArguments arguments)
    {
        _type = type;
        _caller = caller;
        _methodName = methodName;
        _arguments = arguments;
    }

    public TypedMethodCall(IScopeAwareMethodCall methodCall) :
        this(
            TypeIrrealizable.MakeTypedExpression(methodCall.Caller())
                .Type().MethodCallReturnType(methodCall),
            TypeIrrealizable.MakeTypedExpression(methodCall.Caller()),
            methodCall.MethodName().Value(),
            new TypesAwareArguments(methodCall.Arguments())){}
    
    public Type Type()
    {
        return _type;
    }

    public ITypedExpression Caller()
    {
        return _caller;
    }

    public string MethodName()
    {
        return _methodName;
    }

    public ITypesAwareArguments Arguments()
    {
        return _arguments;
    }

    public IList<string> GetRepr()
    {
        return new DeclarationAwareTree.Statement.Expression.MethodCall(this).GetRepr();
    }
}