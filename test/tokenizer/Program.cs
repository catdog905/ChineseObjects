using ChineseObjects.Lang;
using ChineseObjects.Lang.CodeGen;
using ChineseObjects.Lang.Declaration;
using ChineseObjects.Lang.Native;

var calculator = new LangParser();
ChineseObjects.Lang.Program program = calculator.Parse(File.ReadAllText("test/tokenizer/program_text.txt"));
/*
IHumanReadable hp = program;
foreach (string s in hp.GetRepr())
{
    Console.WriteLine(s);
}
*/

ScopeAwareProgram scopeAwareProgram = new ScopeAwareProgram(NativeTypesDeclarations.GlobalScope, program);
TypesAwareProgram typesAwareProgram = new TypesAwareProgram(scopeAwareProgram);


var nativeGen = new LLVMExposingCodeGen();
new LibC().CompileWith(nativeGen);
new Bool().CompileWith(nativeGen);
new Number().CompileWith(nativeGen);

var gen = new CompiledProgram(nativeGen, typesAwareProgram);
