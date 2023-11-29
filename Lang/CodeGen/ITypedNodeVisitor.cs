namespace ChineseObjects.Lang.CodeGen;

public interface ITypedNodeVisitor<T>
{
    T Visit(TypedParameter _);
    T Visit(Declaration.TypedVariable _);
    T Visit(TypedArgument _);
    T Visit(TypedBoolLiteral _);
    T Visit(TypedClassInstantiation _);
    T Visit(TypedMethodCall _);
    T Visit(TypedNumLiteral _);
    T Visit(TypedReference _);
}
