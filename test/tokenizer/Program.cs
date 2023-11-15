// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using ChineseObjects.Lang;

var calculator = new LangParser();
ChineseObjects.Lang.Program program = calculator.Parse(File.ReadAllText("tokenizer/program_text.txt"));
IHumanReadable hp = program;
foreach (string s in hp.GetRepr())
{
    Console.WriteLine(s);
}

ScopeAwareProgram scopeAwareProgram = new ScopeAwareProgram(new Scope(), program);
