using LLVMSharp.Interop;

namespace ChineseObjects.Lang.Native;

public class Number : INativeEntityCompiler
{
    public void CompileWith(CodeGen.LLVMExposingCodeGen g)
    {
        var ctx = g.ctx;
        var Struct = g.Struct;

        var Number = Struct["Number"] = ctx.CreateNamedStruct("Number");
        Number.StructSetBody(new[] { ctx.DoubleType }, Packed: false);

        BuildNegate(g);
    }

    private void BuildNegate(CodeGen.LLVMExposingCodeGen g)
    {
        var ctx = g.ctx;
        var module = g.module;
        var builder = g.builder;
        var FuncType = g.FuncType;
        var Struct = g.Struct;

        var Number = Struct["Number"];
        var PNumber = LLVMTypeRef.CreatePointer(Number, 0);

        const string funcN = "Number.Negate..Number";
        LLVMValueRef func = module.GetNamedFunction(funcN);
        if (func.BasicBlocks.Length != 0)
        {
            throw new CodeGen.LLVMGenException("Function " + funcN + " already has a body");
        }

        var parames = new LLVMTypeRef[]
        {
            /*this*/PNumber
        };

        var funcT = FuncType[funcN] = LLVMTypeRef.CreateFunction(PNumber, parames);
        func = module.AddFunction(funcN, funcT);
        func.Linkage = LLVMLinkage.LLVMExternalLinkage;

        var pThis = func.GetParam(0);
        pThis.Name = "this";

        builder.PositionAtEnd(func.AppendBasicBlock("entry"));
        var u1 = builder.BuildStructGEP2(Number, pThis, 0, "unbox1");
        var v1 = builder.BuildLoad2(ctx.DoubleType, u1, "val1");
        LLVMValueRef res = builder.BuildFNeg(v1, "negate"); // Negate the number
        /* resptr points both to the structure and to its initial field */
        LLVMValueRef resptr = builder.BuildMalloc(Number, "resptr");
        builder.BuildStore(res, resptr);
        builder.BuildRet(resptr);

        func.VerifyFunction(LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }
}
