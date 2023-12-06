using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;

// The base class for all expressions
public interface IExpression : IStatement {}

public class ExpressionWrapper : IExpression
{
    private readonly IExpression _expression;

    public ExpressionWrapper(IExpression expression)
    {
        _expression = expression;
    }

    public ExpressionWrapper(ITypedExpression expression) :
        this(expression switch
        {
            ITypedBoolLiteral boolLiteral => new BoolLiteral(boolLiteral),
            ITypedClassInstantiation classInstantiation => new ClassInstantiation(classInstantiation),
            ITypedReference identifier => new Reference(identifier),
            ITypedMethodCall methodCall => new MethodCall(methodCall),
            ITypedNumLiteral numLiteral => new NumLiteral(numLiteral),
            ITypedThis typedThis => new This(),
            _ => throw new NotImplementedException("Implementation of expression not found")
        })
    {}

    public IList<string> GetRepr()
    {
        return _expression.GetRepr();
    }
}