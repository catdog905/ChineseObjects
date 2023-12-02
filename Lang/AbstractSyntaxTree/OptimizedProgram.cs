using System.Collections.Immutable;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration.Parameter;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement.Expression;

namespace ChineseObjects.Lang.AbstractSyntaxTree;

public class OptimizedProgram
{
    private ITypesAwareProgram _program;

    public OptimizedProgram(ITypesAwareProgram program)
    {
        _program = program;
    }

    public ITypesAwareProgram WithoutUsedVariables()
    {
        return (ITypesAwareProgram)WithoutUnusedVariables(_program).Item2;
    }
    
    private (ImmutableList<string>, ITypesAwareAstNode) WithoutUnusedVariables(ITypesAwareAstNode astNode)
    {
        switch (astNode)
        {
            case ITypesAwareProgram program:
                return
                (
                    program.ClassDeclarations()
                        .Aggregate(
                            ImmutableList<string>.Empty,
                            (acc, cur) => acc.AddRange(WithoutUnusedVariables(cur).Item1)),
                    new TypesAwareProgram(
                        program.ClassDeclarations()
                            .Select(decl => (TypesAwareClassDeclaration)WithoutUnusedVariables(decl).Item2))
                );
            case ITypesAwareClassDeclaration classDeclaration:
                var usedFields = classDeclaration.ConstructorDeclarations()
                    .Aggregate(
                        ImmutableList<string>.Empty,
                        (acc, cur) => acc.AddRange(WithoutUnusedVariables(cur).Item1))
                    .AddRange(
                        classDeclaration.MethodDeclarations()
                            .Aggregate(
                                ImmutableList<string>.Empty,
                                (acc, cur) => acc.AddRange(WithoutUnusedVariables(cur).Item1)));
                return
                (
                    usedFields,
                    classDeclaration.WithoutVariables(
                        classDeclaration.VariableDeclarations()
                            .Where(decl => !usedFields.Contains(decl.Name()))
                            .Select(decl => decl.Name())
                            .ToList())
                );
            case ITypesAwareMethod method:
                return
                (
                    method.Parameters().GetParameters()
                        .Aggregate(
                            ImmutableList<string>.Empty,
                            (acc, cur) => acc.AddRange(WithoutUnusedVariables(cur).Item1))
                        .AddRange(WithoutUnusedVariables(method.Body()).Item1),
                    method
                );
            case ITypesAwareConstructor constructor:
                return
                (
                    constructor.Parameters().GetParameters()
                        .Aggregate(
                            ImmutableList<string>.Empty,
                            (acc, cur) => acc.AddRange(WithoutUnusedVariables(cur).Item1))
                        .AddRange(WithoutUnusedVariables(constructor.Body()).Item1),
                    constructor
                );
            case ITypedVariable variable:
                return
                (
                    ImmutableList<string>.Empty,
                    variable
                );
            case ITypedParameter parameter:
                return
                (
                    ImmutableList<string>.Empty,
                    parameter
                );
            case ITypedArgument argument:
                return
                (
                    WithoutUnusedVariables(argument.Value()).Item1,
                    argument
                );
            case ITypesAwareStatementsBlock statementsBlock:

                (ImmutableList<string>, ITypesAwareStatementsBlock) iterateOverStatementBlock(
                    ITypesAwareStatementsBlock statementsBlock)
                {
                    if (statementsBlock.Statements().Count() == 0)
                        return (ImmutableList<string>.Empty, new TypesAwareStatementsBlock());
                    ITypesAwareStatement head = statementsBlock.Statements().First();
                    TypesAwareStatementsBlock tail =
                        new TypesAwareStatementsBlock(statementsBlock.Statements().Skip(1));
                    var (tailVars, tailStatementBlock) = iterateOverStatementBlock(tail);
                    if (head is TypesAwareAssignment assignment)
                    {
                        if (!tailVars.Contains(assignment.Name()))
                        {
                            return (tailVars, tailStatementBlock);
                        }

                        var (newUsedVars, newAssignment)
                            = WithoutUnusedVariables(assignment);
                        return
                        (
                            tailVars.AddRange(newUsedVars),
                            new TypesAwareStatementsBlock(
                                (ITypesAwareAssignment)newAssignment,
                                tailStatementBlock
                            )
                        );
                    }
                    else
                    {
                        var (newUsedVars, newHead)
                            = WithoutUnusedVariables(head);
                        return
                        (
                            tailVars.AddRange(newUsedVars),
                            new TypesAwareStatementsBlock(
                                (ITypesAwareStatement)newHead,
                                tailStatementBlock
                            )
                        );
                    }
                }

                return iterateOverStatementBlock(statementsBlock);
            case ITypesAwareStatement statement:
                switch (statement)
                {
                    case ITypesAwareAssignment assignment:
                        return
                        (
                            WithoutUnusedVariables(assignment.Expr()).Item1,
                            assignment
                        );
                    case ITypesAwareIfElse ifElse:
                        ImmutableList<string> usedVariables = WithoutUnusedVariables(ifElse.Condition()).Item1
                            .AddRange(WithoutUnusedVariables(ifElse.Then()).Item1);
                        if (ifElse.Else() != null)
                            usedVariables = usedVariables.AddRange(WithoutUnusedVariables(ifElse.Else()).Item1);
                        return
                        (
                            usedVariables,
                            ifElse
                        );
                    case ITypesAwareReturn @return:
                        return
                        (
                            WithoutUnusedVariables(@return.Expression()).Item1,
                            @return
                        );
                    case ITypesAwareWhile @while:
                        return
                        (
                            WithoutUnusedVariables(@while.Condition()).Item1
                                .AddRange(
                                    WithoutUnusedVariables(@while.Body()).Item1),
                            @while
                        );
                    case ITypedExpression expression:
                        switch (expression)
                        {
                            case ITypedBoolLiteral boolLiteral:
                                return
                                (
                                    ImmutableList<string>.Empty,
                                    boolLiteral
                                );
                            case ITypedClassInstantiation classInstantiation:
                                return
                                (
                                    classInstantiation.Arguments().Values()
                                        .Aggregate(
                                            ImmutableList<string>.Empty,
                                            (acc, cur) =>
                                                acc.AddRange(WithoutUnusedVariables(cur).Item1)),
                                    classInstantiation
                                );
                            case ITypedMethodCall methodCall:
                                return
                                (
                                    WithoutUnusedVariables(methodCall.Caller()).Item1
                                        .AddRange(
                                            methodCall.Arguments().Values()
                                                .Aggregate(
                                                    ImmutableList<string>.Empty, 
                                                    (acc, cur) => 
                                                        acc.AddRange(
                                                            WithoutUnusedVariables(cur).Item1))),
                                    methodCall
                                );
                            case ITypedNumLiteral numLiteral:
                                return
                                (
                                    ImmutableList<string>.Empty,
                                    numLiteral
                                );
                            case ITypedReference typedReference:
                                return
                                (
                                    new List<string> { typedReference.Name() }.ToImmutableList(),
                                    typedReference
                                );
                            default:
                                throw new NotImplementedException();
                        }
                    default:
                        throw new NotImplementedException();
                }
            default:
                throw new NotImplementedException();
        }
    }
}