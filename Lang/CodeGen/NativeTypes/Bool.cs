using LLVMSharp.Interop;

namespace ChineseObjects.Lang.Native;

public class Bool : INativeEntityCompiler
{
    public void CompileWith(CodeGen.LLVMExposingCodeGen g)
    {
        var ctx = g.ctx;
        var Struct = g.Struct;

        var Bool = Struct["Bool"] = ctx.CreateNamedStruct("Bool");
        Bool.StructSetBody(new[] { ctx.Int1Type }, Packed: false);

        BuildAnd(g);
        BuildTerminateExecution(g);
    }

    private void BuildAnd(CodeGen.LLVMExposingCodeGen g)
    {
        var ctx = g.ctx;
        var module = g.module;
        var builder = g.builder;
        var FuncType = g.FuncType;
        var Struct = g.Struct;

        var Bool = Struct["Bool"];
        var PBool = LLVMTypeRef.CreatePointer(Bool, 0);

        const string funcN = "Bool.And..Bool";
        LLVMValueRef func = module.GetNamedFunction(funcN);
        if (func.BasicBlocks.Length != 0)
        {
            throw new CodeGen.LLVMGenException("Function " + funcN + " already has a body");
        }

        var parames = new LLVMTypeRef[]
        {
            /*this*/PBool,
            /*other*/PBool
        };

        var funcT = FuncType[funcN] = LLVMTypeRef.CreateFunction(PBool, parames);
        func = module.AddFunction(funcN, funcT);
        func.Linkage = LLVMLinkage.LLVMExternalLinkage;

        var pThis = func.GetParam(0);
        pThis.Name = "this";
        var pOther = func.GetParam(1);
        pOther.Name = "other";

        builder.PositionAtEnd(func.AppendBasicBlock("entry"));
        var u1 = builder.BuildStructGEP2(Bool, pThis, 0, "unbox1");
        var u2 = builder.BuildStructGEP2(Bool, pOther, 0, "unbox2");
        var v1 = builder.BuildLoad2(ctx.Int1Type, u1, "val1");
        var v2 = builder.BuildLoad2(ctx.Int1Type, u2, "val2");
        LLVMValueRef res = builder.BuildAnd(v1, v2, "and");
        /* resptr points both to the structure and to its initial field */
        LLVMValueRef resptr = builder.BuildMalloc(Bool, "resptr");
        builder.BuildStore(res, resptr);
        builder.BuildRet(resptr);

        func.VerifyFunction(LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }

    private void BuildTerminateExecution(CodeGen.LLVMExposingCodeGen g)
    {
        var ctx = g.ctx;
        var module = g.module;
        var builder = g.builder;
        var FuncType = g.FuncType;
        var Struct = g.Struct;

        var Bool = Struct["Bool"];
        var PBool = LLVMTypeRef.CreatePointer(Bool, 0);

        const string funcN = "Bool.TerminateExecution..";
        var func = module.GetNamedFunction(funcN);
        if (func.BasicBlocks.Length != 0)
        {
            throw new CodeGen.LLVMGenException("Function " + funcN + " already has a body");
        }

        var parames = new LLVMTypeRef[]
        {
            /*this*/PBool
        };

        var funcT = FuncType[funcN] = LLVMTypeRef.CreateFunction(PBool, parames);
        func = module.AddFunction(funcN, funcT);
        var pThis = func.GetParam(0);
        pThis.Name = "this";

        builder.PositionAtEnd(func.AppendBasicBlock("entry"));
        var unboxed = builder.BuildStructGEP2(Bool, pThis, 0, "unboxed");
        builder.BuildCall2(FuncType["exit"], module.GetNamedFunction("exit"),
            new[] { builder.BuildIntCast(builder.BuildLoad2(ctx.Int1Type, unboxed), ctx.Int32Type) });
        builder.BuildRet(LLVMValueRef.CreateConstPointerNull(PBool));
       
        func.VerifyFunction(LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }
}
