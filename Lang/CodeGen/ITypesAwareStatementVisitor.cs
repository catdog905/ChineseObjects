namespace ChineseObjects.Lang.CodeGen;

public interface ITypesAwareStatementVisitor<T>
{
    T Visit(TypedParameter _);
    T Visit(Declaration.TypedVariable _);
    T Visit(TypedArgument _);
    T Visit(TypedBoolLiteral _);
    T Visit(TypedClassInstantiation _);
    T Visit(TypedMethodCall _);
    T Visit(TypedNumLiteral _);
    T Visit(TypedReference _);
    T Visit(ITypesAwareReturn _);
    T Visit(ITypesAwareWhile _);
    T Visit(ITypesAwareIfElse _);
    T Visit(ITypesAwareAssignment _);
}
