// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var calculator = new ChineseObjects.Lang.LangParser();
calculator.Parse(File.ReadAllText("program_text.txt"));
