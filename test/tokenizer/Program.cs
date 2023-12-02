// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using ChineseObjects.Lang;
using ChineseObjects.Lang.AbstractSyntaxTree;
using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;

var calculator = new LangParser();
ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration.Program program = calculator.Parse(File.ReadAllText("tokenizer/program_text.txt"));
IHumanReadable hp = program;
foreach (string s in hp.GetRepr())
{
    Console.WriteLine(s);
}

ScopeAwareProgram scopeAwareProgram = new ScopeAwareProgram(new Scope(), program);
TypesAwareProgram typesAwareProgram = new TypesAwareProgram(scopeAwareProgram);
ITypesAwareProgram programWithoutUnusedVariables = new OptimizedProgram(typesAwareProgram).WithoutUsedVariables();
Console.WriteLine(scopeAwareProgram.Scope());

foreach (string s in programWithoutUnusedVariables.GetRepr())
{
    Console.WriteLine(s);
}