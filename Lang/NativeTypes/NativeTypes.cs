using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public static class NativeTypes
{
    private class NativeMethod : IStatementsBlock
    {
        public IList<string> GetRepr() => throw new NotImplementedException();
        public IEnumerable<IStatement> Statements() => throw new NotImplementedException();
    }
    
    private static readonly MethodDeclaration BoolAnd = new MethodDeclaration(
        new Identifier("And"), new Parameters(new Parameter(new Identifier("other"), new Identifier("Bool"))),
        new Identifier("Bool"), new NativeMethod());

    private static readonly MethodDeclaration BoolTerminateExecution = new MethodDeclaration(
        new Identifier("TerminateExecution"), new Parameters(), new Identifier("Bool"), new NativeMethod());
    
    private static readonly ClassDeclaration BoolDeclaration = new ClassDeclaration(new Identifier("Bool"),
        ImmutableList<IIdentifier>.Empty, ImmutableList<IConstructorDeclaration>.Empty,
        ImmutableList<IVariableDeclaration>.Empty, new []{BoolAnd, BoolTerminateExecution});
    
    // private static readonly MethodDeclaration

    public static readonly Type Bool = new Type(BoolDeclaration);

    public static readonly Scope GlobalScope = new Scope(ImmutableDictionary<string, Type>.Empty.Add("Bool", Bool),
        ImmutableDictionary<string, Entity>.Empty);
}
