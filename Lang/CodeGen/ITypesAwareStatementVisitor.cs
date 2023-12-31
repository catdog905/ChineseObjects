using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration.Parameter;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

namespace ChineseObjects.Lang.CodeGen;

public interface ITypesAwareStatementVisitor<T>
{
    T Visit(ITypedArgument _);
    T Visit(ITypedBoolLiteral _);
    T Visit(ITypedClassInstantiation _);
    T Visit(ITypedMethodCall _);
    T Visit(ITypedNumLiteral _);
    T Visit(ITypedReference _);
    T Visit(ITypesAwareReturn _);
    T Visit(ITypesAwareWhile _);
    T Visit(ITypesAwareIfElse _);
    T Visit(ITypesAwareAssignment _);
    T Visit(ITypedThis _);
}
