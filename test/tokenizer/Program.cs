using ChineseObjects.Lang;
using ChineseObjects.Lang.AbstractSyntaxTree;
using ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree;
using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;
using ChineseObjects.Lang.CodeGen;
using ChineseObjects.Lang.Native;
using LangParser = ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.LangParser;

var calculator = new LangParser();
ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration.Program program = calculator.Parse(File.ReadAllText("test/tokenizer/program_text.txt"));
/*
IHumanReadable hp = program;
foreach (string s in hp.GetRepr())
{
    Console.WriteLine(s);
}
*/

ScopeAwareProgram scopeAwareProgram = new ScopeAwareProgram(NativeTypesDeclarations.GlobalScope, program);
TypesAwareProgram typesAwareProgram = new TypesAwareProgram(scopeAwareProgram);
ITypesAwareProgram programWithoutUnusedVariables = new OptimizedProgram(typesAwareProgram).WithoutUnusedVariables();
Console.WriteLine(scopeAwareProgram.Scope());

foreach (string s in programWithoutUnusedVariables.GetRepr())
{
    Console.WriteLine(s);
}


var nativeGen = new LLVMExposingCodeGen();
new LibC().CompileWith(nativeGen);
new Bool().CompileWith(nativeGen);
new Number().CompileWith(nativeGen);

var gen = new CompiledProgram(nativeGen, programWithoutUnusedVariables);
gen.MakeExecutable();
