using System.Collections.Immutable;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Statement.Expression;
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

    public ITypesAwareProgram WithoutUnusedVariables()
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
                            .Select(decl =>
                                (TypesAwareClassDeclaration)WithoutUnusedVariables(decl).Item2))
                );
            case ITypesAwareClassDeclaration classDeclaration:
                var usedFields = 
                    classDeclaration.ConstructorDeclarations()
                    .Aggregate(
                        ImmutableList<string>.Empty,
                        (acc, cur) =>
                            acc.AddRange(WithoutUnusedVariables(cur).Item1))
                    .AddRange(
                        classDeclaration.MethodDeclarations()
                            .Aggregate(
                                ImmutableList<string>.Empty,
                                (acc, cur) =>
                                    acc.AddRange(WithoutUnusedVariables(cur).Item1)));
                return
                (
                    usedFields,
                    new TypesAwareClassDeclaration(
                        classDeclaration.SelfType(),
                        classDeclaration.ParentClassNames(),
                        classDeclaration.ConstructorDeclarations()
                            .Select(decl => (TypesAwareConstructor)WithoutUnusedVariables(decl).Item2),
                        classDeclaration.VariableDeclarations(),
                        classDeclaration.MethodDeclarations()
                            .Select(decl => (TypesAwareMethod)WithoutUnusedVariables(decl).Item2)
                        ).WithoutVariables(
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
                            (acc, cur) =>
                                acc.AddRange(WithoutUnusedVariables(cur).Item1))
                        .AddRange(WithoutUnusedVariables(method.Body()).Item1),
                    new TypesAwareMethod(
                        method.MethodName(),
                        new TypesAwareParameters(
                            method.Parameters().GetParameters()
                                .Aggregate(
                                    ImmutableList<ITypedParameter>.Empty,
                                    (acc, cur) =>
                                        acc.Add((ITypedParameter)WithoutUnusedVariables(cur).Item2))),
                        method.ReturnType(),
                        (ITypesAwareStatementsBlock)WithoutUnusedVariables(method.Body()).Item2)
                );
            case ITypesAwareConstructor constructor:
                return
                (
                    constructor.Parameters().GetParameters()
                        .Aggregate(
                            ImmutableList<string>.Empty,
                            (acc, cur) => acc.AddRange(WithoutUnusedVariables(cur).Item1))
                        .AddRange(WithoutUnusedVariables(constructor.Body()).Item1),
                    new TypesAwareConstructor(
                        new TypesAwareParameters(
                            constructor.Parameters().GetParameters()
                                .Aggregate(
                                    ImmutableList<ITypedParameter>.Empty,
                                    (acc, cur) =>
                                        acc.Add((ITypedParameter)WithoutUnusedVariables(cur).Item2))
                        ),
                        (ITypesAwareStatementsBlock)WithoutUnusedVariables(constructor.Body()).Item2
                    )
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
                    new TypedArgument((ITypedExpression)WithoutUnusedVariables(argument.Value()).Item2)
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
                            tailVars.Remove(assignment.Name()).AddRange(newUsedVars),
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
                            new TypesAwareAssignment(
                                assignment.Name(),
                                (ITypedExpression)WithoutUnusedVariables(assignment.Expr()).Item2
                                )
                        );
                    case ITypesAwareIfElse ifElse:
                        ImmutableList<string> usedVariables = WithoutUnusedVariables(ifElse.Condition()).Item1
                            .AddRange(WithoutUnusedVariables(ifElse.Then()).Item1);
                        if (ifElse.Else() != null)
                            usedVariables = usedVariables.AddRange(WithoutUnusedVariables(ifElse.Else()).Item1);
                        return
                        (
                            usedVariables,
                            new TypesAwareIfElse(
                                (ITypedExpression)WithoutUnusedVariables(ifElse.Condition()).Item2,
                                (ITypesAwareStatementsBlock)WithoutUnusedVariables(ifElse.Then()).Item2,
                                ifElse.Else() == null ? 
                                    null : 
                                    (ITypesAwareStatementsBlock?)WithoutUnusedVariables(ifElse.Else()).Item2
                                )
                        );
                    case ITypesAwareReturn @return:
                        return
                        (
                            WithoutUnusedVariables(@return.Expression()).Item1,
                            new TypesAwareReturn((ITypedExpression)WithoutUnusedVariables(@return.Expression()).Item2)
                        );
                    case ITypesAwareWhile @while:
                        return
                        (
                            WithoutUnusedVariables(@while.Condition()).Item1
                                .AddRange(
                                    WithoutUnusedVariables(@while.Body()).Item1),
                            new TypesAwareWhile(
                                (ITypedExpression)WithoutUnusedVariables(@while.Condition()).Item2,
                                (ITypesAwareStatementsBlock)WithoutUnusedVariables(@while.Body()).Item2)
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
                                    new TypedClassInstantiation(
                                        classInstantiation.Type(),
                                        classInstantiation.ClassName(),
                                        new TypesAwareArguments(
                                            classInstantiation.Arguments().Values()
                                                .Aggregate(
                                                    ImmutableList<ITypedArgument>.Empty,
                                                    (acc, cur) =>
                                                        acc.Add((ITypedArgument)WithoutUnusedVariables(cur).Item2))
                                        )
                                    )
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
                                    new TypedMethodCall(
                                        methodCall.Type(),
                                        (ITypedExpression)WithoutUnusedVariables(methodCall.Caller()).Item2,
                                        methodCall.MethodName(),
                                        new TypesAwareArguments(
                                            methodCall.Arguments().Values()
                                                .Aggregate(
                                                    ImmutableList<ITypedArgument>.Empty,
                                                    (acc, cur) =>
                                                        acc.Add(
                                                            (ITypedArgument)WithoutUnusedVariables(cur).Item2))
                                        )
                                    )
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
                            case TypedThis typedThis:
                                return
                                (
                                    ImmutableList<string>.Empty,
                                    typedThis
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