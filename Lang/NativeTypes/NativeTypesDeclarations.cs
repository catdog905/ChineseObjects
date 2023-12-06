using System.Collections.Immutable;

namespace ChineseObjects.Lang;

/// <summary>
/// Declares native types.
/// </summary>
public static class NativeTypesDeclarations
{
    private class NativeMethod : IStatementsBlock
    {
        public IList<string> GetRepr() => throw new NotImplementedException();
        public IEnumerable<IStatement> Statements() => throw new NotImplementedException();
    }

    // Bool Declarations
    private static readonly MethodDeclaration BoolAnd = new MethodDeclaration(
        new Identifier("And"), new Parameters(new Parameter(new Identifier("other"), new Identifier("Bool"))),
        new Identifier("Bool"), new NativeMethod());

    private static readonly MethodDeclaration BoolOr = new MethodDeclaration(
        new Identifier("Or"), new Parameters(new Parameter(new Identifier("other"), new Identifier("Bool"))),
        new Identifier("Bool"), new NativeMethod());

    private static readonly MethodDeclaration BoolXor = new MethodDeclaration(
        new Identifier("Xor"), new Parameters(new Parameter(new Identifier("other"), new Identifier("Bool"))),
        new Identifier("Bool"), new NativeMethod());

    private static readonly MethodDeclaration BoolNot = new MethodDeclaration(
        new Identifier("Not"), new Parameters(), new Identifier("Bool"), new NativeMethod());

    private static readonly MethodDeclaration BoolToInteger = new MethodDeclaration(
        new Identifier("ToInteger"), new Parameters(), new Identifier("Int"), new NativeMethod());


    private static readonly MethodDeclaration BoolTerminateExecution = new MethodDeclaration(
        new Identifier("TerminateExecution"), new Parameters(), new Identifier("Bool"), new NativeMethod());

    private static readonly ClassDeclaration BoolDeclaration = new ClassDeclaration(new Identifier("Bool"),
        ImmutableList<IIdentifier>.Empty, ImmutableList<IConstructorDeclaration>.Empty,
        ImmutableList<IVariableDeclaration>.Empty, new[] { BoolAnd, BoolOr, BoolXor, BoolNot, BoolToInteger, BoolTerminateExecution });

    public static readonly Type Bool = new Type(BoolDeclaration);

    // Number declarations
    private static readonly MethodDeclaration NumberNegate = new MethodDeclaration(
    new Identifier("Negate"), new Parameters(), new Identifier("Number"), new NativeMethod());

    private static readonly MethodDeclaration NumberPlus = new MethodDeclaration(
    new Identifier("Plus"), new Parameters(new Parameter(new Identifier("other"), new Identifier("Number"))),
    new Identifier("Number"), new NativeMethod());

    private static readonly MethodDeclaration NumberMinus = new MethodDeclaration(
    new Identifier("Minus"), new Parameters(new Parameter(new Identifier("other"), new Identifier("Number"))),
    new Identifier("Number"), new NativeMethod());

    private static readonly MethodDeclaration NumberMult = new MethodDeclaration(
    new Identifier("Mult"), new Parameters(new Parameter(new Identifier("other"), new Identifier("Number"))),
    new Identifier("Number"), new NativeMethod());
    
    private static readonly ClassDeclaration NumberDeclaration = new ClassDeclaration(new Identifier("Number"),
    ImmutableList<IIdentifier>.Empty, ImmutableList<IConstructorDeclaration>.Empty,
    ImmutableList<IVariableDeclaration>.Empty, new[] { NumberNegate, NumberPlus, NumberMinus, NumberMult });

    public static readonly Type Number = new Type(NumberDeclaration);
    
    /// <summary>
    /// Scope that contains all native types.
    /// </summary>
    public static readonly Scope GlobalScope = new Scope(
    ImmutableDictionary<string, Type>.Empty.Add("Bool", Bool).Add("Number", Number),
    ImmutableDictionary<string, Entity>.Empty);
}
