// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using ChineseObjects.Lang;
using ChineseObjects.Lang.Declaration;
using Type = ChineseObjects.Lang.Type;

var calculator = new LangParser();
ChineseObjects.Lang.Program program = calculator.Parse(File.ReadAllText("tokenizer/multi_class.txt"));
IHumanReadable hp = program;
foreach (string s in hp.GetRepr())
{
    Console.WriteLine(s);
}

ScopeAwareProgram scopeAwareProgram = new ScopeAwareProgram(new Scope(), program);
TypesAwareProgram typesAwareProgram = new TypesAwareProgram(scopeAwareProgram);
Console.WriteLine(scopeAwareProgram.Scope());