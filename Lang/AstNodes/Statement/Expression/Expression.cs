namespace ChineseObjects.Lang;

// The base class for all expressions
public interface Expression : Statement {
    /*
     * Given the current scope, attempts to evaluate itself and deduce the type of the result.
     * Returns the class of the result in case of success, `null` otherwise.
     *
     * The `null` return value indicates that the expression can not be evaluated with the
     * given scope.
     */
    ClassDeclaration? EvaluatedType(Scope scope);
}
