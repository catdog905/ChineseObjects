// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

var calculator = new ChineseObjects.Lang.LangParser();
Console.WriteLine(calculator.Parse(File.ReadAllText("test/tokenizer/program_text.txt")));
