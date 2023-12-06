namespace ChineseObjects.Lang.CodeGen;

public interface ITypesAwareStatementVisitor<T>
{
    T Visit(Declaration.ITypedVariable _);
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
    T Visit(Declaration.ITypedThis _);
}
