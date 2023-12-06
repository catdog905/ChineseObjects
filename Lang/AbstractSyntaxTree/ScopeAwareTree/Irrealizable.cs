using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;
using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree;

public class Irrealizable
{
    public static IScopeAwareExpression MakeScopeAware(Scope scope, IExpression expression)
    {
        return expression switch
        {
            IBoolLiteral boolLiteral => new ScopeAwareBoolLiteral(scope, boolLiteral),
            IClassInstantiation classInstantiation => new ScopeAwareClassInstantiation(scope, classInstantiation),
            Reference reference => new ScopeAwareReference(scope, reference),
            IMethodCall methodCall => new ScopeAwareMethodCall(scope, methodCall),
            INumLiteral numLiteral => new ScopeAwareNumLiteral(scope, numLiteral),
            IThis _ => new ScopeAwareThis(scope),
            _ => throw new NotImplementedException("Implementation of expression not found")
        };
    }

    public static IScopeAwareStatement MakeScopeAware(Scope scope, IStatement statement)
    {
        return statement switch
        {
            IAssignment assignment => new ScopeAwareAssignment(scope, assignment),
            IIfElse ifElse => new ScopeAwareIfElse(scope, ifElse),
            IReturn @return => new ScopeAwareReturn(scope, @return),
            IWhile @while => new ScopeAwareWhile(scope, @while),
            IExpression expression => MakeScopeAware(scope, expression),
            _ => throw new NotImplementedException("Implementation of statement not found")
        };
    }
}
