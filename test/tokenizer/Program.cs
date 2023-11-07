// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

var calculator = new ChineseObjects.Lang.LangParser();
ChineseObjects.Lang.IAstNode program = calculator.Parse(File.ReadAllText("test/tokenizer/Fibonacci.txt"));
ChineseObjects.Lang.IHumanReadable hp = (ChineseObjects.Lang.IHumanReadable)program;
foreach (string s in hp.GetRepr())
{
    Console.WriteLine(s);
}
