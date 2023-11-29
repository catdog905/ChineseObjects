using System.Collections.Immutable;
using ChineseObjects.Lang;
using ChineseObjects.Lang.CodeGen;
using ChineseObjects.Lang.Declaration;
using LLVMSharp;
using Type = ChineseObjects.Lang.Type;

var calculator = new LangParser();
ChineseObjects.Lang.Program program = calculator.Parse(File.ReadAllText("tokenizer/program_text.txt"));
IHumanReadable hp = program;
foreach (string s in hp.GetRepr())
{
    Console.WriteLine(s);
}

ScopeAwareProgram scopeAwareProgram = new ScopeAwareProgram(new Scope(), program);
TypesAwareProgram typesAwareProgram = new TypesAwareProgram(scopeAwareProgram);
Console.WriteLine(scopeAwareProgram.Scope());

// .

var dbl = new Type(new ClassDeclaration(new Identifier("double"), new List<IIdentifier>(), new List<IConstructorDeclaration>(),
    new List<IVariableDeclaration>(), new List<IMethodDeclaration>()));
var scope = new Scope(ImmutableDictionary<string, Type>.Empty.Add("double", dbl), ImmutableDictionary<string, Entity>.Empty);
ITypedExpression numLit = new TypedNumLiteral(dbl, 1.5);
var gen = new LLVMValGen();
Console.WriteLine();
numLit.AcceptVisitor(gen).Dump();
Console.WriteLine();


// +
LLVMModuleRef module = LLVM.ModuleCreateWithName("hello");
LLVMBuilderRef builder = LLVM.CreateBuilder();

LLVMValueRef constant = LLVM.ConstReal(LLVM.IntType(4), 55);
LLVMValueRef anotherConstant = LLVM.ConstReal(LLVM.IntType(4), 0);
LLVMValueRef added = LLVM.BuildFAdd(builder, constant, anotherConstant, "zerosum");

LLVMValueRef func = LLVM.GetNamedFunction(module, "SumConsts");
if (func.Pointer != IntPtr.Zero)
{
    Console.WriteLine("Unexpected: function has already been declared");
    if (LLVM.CountBasicBlocks(func) != 0)
    {
        Console.WriteLine("Unexpected: function already has body!");
    }
}

LLVMTypeRef unusedArg1, unusedArg2;
unusedArg1 = unusedArg2 = LLVM.Int8Type();

func = LLVM.AddFunction(module, "SumConsts",
    LLVM.FunctionType(LLVM.DoubleType(), new[] { unusedArg1, unusedArg2 }, false));
LLVM.SetLinkage(func, LLVMLinkage.LLVMExternalLinkage);
LLVM.SetValueName(LLVM.GetParam(func, 0), "iA");
LLVM.SetValueName(LLVM.GetParam(func, 1), "iB");

LLVM.PositionBuilderAtEnd(builder, LLVM.AppendBasicBlock(func, "entry"));

// Function body...

LLVMValueRef conv = LLVM.BuildSIToFP(builder, added, LLVM.DoubleType(), "conv");
LLVM.BuildRet(builder, conv);
LLVM.VerifyFunction(func, LLVMVerifierFailureAction.LLVMPrintMessageAction);

func = LLVM.GetNamedFunction(module, "SumConsts");

LLVM.DumpValue(func);
