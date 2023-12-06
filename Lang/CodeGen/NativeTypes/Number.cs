using LLVMSharp.Interop;

namespace ChineseObjects.Lang.Native;

public class Number : INativeEntityCompiler
{
    public void CompileWith(CodeGen.LLVMExposingCodeGen g)
    {
        var ctx = g.ctx;
        var Struct = g.Struct;

        var Number = Struct["Number"] = ctx.CreateNamedStruct("Number");
        Number.StructSetBody(new[] { ctx.Int32Type }, Packed: false);

        BuildPrint(g);
        BuildNegate(g);
        BuildPlus(g);
        BuildMinus(g);
        BuildMult(g);
    }

    private void BuildPrint(CodeGen.LLVMExposingCodeGen g)
    {
        var ctx = g.ctx;
        var module = g.module;
        var builder = g.builder;
        var FuncType = g.FuncType;
        var Struct = g.Struct;

        var Number = Struct["Number"];
        var PNumber = LLVMTypeRef.CreatePointer(Number, 0);

        const string funcN = "Number.Print..";
        LLVMValueRef func = module.GetNamedFunction(funcN);
        if (func.BasicBlocks.Length != 0)
        {
            throw new CodeGen.LLVMGenException("Function " + funcN + " already has a body");
        }

        var parames = new LLVMTypeRef[]
        {
            /*this*/PNumber
        };

        var funcT = FuncType[funcN] = LLVMTypeRef.CreateFunction(/*returns nil*/g.OpaquePtr, parames);
        func = module.AddFunction(funcN, funcT);
        func.Linkage = LLVMLinkage.LLVMExternalLinkage;

        var pThis = func.GetParam(0);
        pThis.Name = "this";

        builder.PositionAtEnd(func.AppendBasicBlock("entry"));
        var unboxed = builder.BuildLoad2(ctx.Int32Type, pThis, "unboxed");
        
        var intFmt = g.builder.BuildGlobalStringPtr("%d\n\0", "intFmt");
        builder.BuildCall2(FuncType["printf"], module.GetNamedFunction("printf"), new[] { intFmt, unboxed });
        builder.BuildRet(pThis);
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
        var v1 = builder.BuildLoad2(ctx.Int32Type, u1, "val1");
        LLVMValueRef res = builder.BuildNeg(v1, "negate");
        /* resptr points both to the structure and to its initial field */
        LLVMValueRef resptr = builder.BuildMalloc(Number, "resptr");
        builder.BuildStore(res, resptr);
        builder.BuildRet(resptr);

        func.VerifyFunction(LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }

    private void BuildPlus(CodeGen.LLVMExposingCodeGen g)
    {
        var ctx = g.ctx;
        var module = g.module;
        var builder = g.builder;
        var FuncType = g.FuncType;
        var Struct = g.Struct;

        var Number = Struct["Number"];
        var PNumber = LLVMTypeRef.CreatePointer(Number, 0);

        const string funcN = "Number.Plus..Number";
        LLVMValueRef func = module.GetNamedFunction(funcN);
        if (func.BasicBlocks.Length != 0)
        {
            throw new CodeGen.LLVMGenException("Function " + funcN + " already has a body");
        }

        var parames = new LLVMTypeRef[]
        {
            /*this*/PNumber,
            /*other*/PNumber
        };

        var funcT = FuncType[funcN] = LLVMTypeRef.CreateFunction(PNumber, parames);
        func = module.AddFunction(funcN, funcT);
        func.Linkage = LLVMLinkage.LLVMExternalLinkage;

        var pThis = func.GetParam(0);
        pThis.Name = "this";
        var pOther = func.GetParam(1);
        pOther.Name = "other";

        builder.PositionAtEnd(func.AppendBasicBlock("entry"));
        var u1 = builder.BuildStructGEP2(Number, pThis, 0, "unbox1");
        var u2 = builder.BuildStructGEP2(Number, pOther, 0, "unbox2");
        var v1 = builder.BuildLoad2(ctx.Int32Type, u1, "val1");
        var v2 = builder.BuildLoad2(ctx.Int32Type, u2, "val2");
        LLVMValueRef res = builder.BuildAdd(v1, v2, "add");
        /* resptr points both to the structure and to its initial field */
        LLVMValueRef resptr = builder.BuildMalloc(Number, "resptr");
        builder.BuildStore(res, resptr);
        builder.BuildRet(resptr);

        func.VerifyFunction(LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }

    private void BuildMinus(CodeGen.LLVMExposingCodeGen g)
    {
        var ctx = g.ctx;
        var module = g.module;
        var builder = g.builder;
        var FuncType = g.FuncType;
        var Struct = g.Struct;

        var Number = Struct["Number"];
        var PNumber = LLVMTypeRef.CreatePointer(Number, 0);

        const string funcN = "Number.Minus..Number";
        LLVMValueRef func = module.GetNamedFunction(funcN);
        if (func.BasicBlocks.Length != 0)
        {
            throw new CodeGen.LLVMGenException("Function " + funcN + " already has a body");
        }

        var parames = new LLVMTypeRef[]
        {
            /*this*/PNumber,
            /*other*/PNumber
        };

        var funcT = FuncType[funcN] = LLVMTypeRef.CreateFunction(PNumber, parames);
        func = module.AddFunction(funcN, funcT);
        func.Linkage = LLVMLinkage.LLVMExternalLinkage;

        var pThis = func.GetParam(0);
        pThis.Name = "this";
        var pOther = func.GetParam(1);
        pOther.Name = "other";

        builder.PositionAtEnd(func.AppendBasicBlock("entry"));
        var u1 = builder.BuildStructGEP2(Number, pThis, 0, "unbox1");
        var u2 = builder.BuildStructGEP2(Number, pOther, 0, "unbox2");
        var v1 = builder.BuildLoad2(ctx.Int32Type, u1, "val1");
        var v2 = builder.BuildLoad2(ctx.Int32Type, u2, "val2");
        LLVMValueRef res = builder.BuildSub(v1, v2, "sub");
        /* resptr points both to the structure and to its initial field */
        LLVMValueRef resptr = builder.BuildMalloc(Number, "resptr");
        builder.BuildStore(res, resptr);
        builder.BuildRet(resptr);

        func.VerifyFunction(LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }

    private void BuildMult(CodeGen.LLVMExposingCodeGen g)
    {
        var ctx = g.ctx;
        var module = g.module;
        var builder = g.builder;
        var FuncType = g.FuncType;
        var Struct = g.Struct;

        var Number = Struct["Number"];
        var PNumber = LLVMTypeRef.CreatePointer(Number, 0);

        const string funcN = "Number.Mult..Number";
        LLVMValueRef func = module.GetNamedFunction(funcN);
        if (func.BasicBlocks.Length != 0)
        {
            throw new CodeGen.LLVMGenException("Function " + funcN + " already has a body");
        }

        var parames = new LLVMTypeRef[]
        {
            /*this*/PNumber,
            /*other*/PNumber
        };

        var funcT = FuncType[funcN] = LLVMTypeRef.CreateFunction(PNumber, parames);
        func = module.AddFunction(funcN, funcT);
        func.Linkage = LLVMLinkage.LLVMExternalLinkage;

        var pThis = func.GetParam(0);
        pThis.Name = "this";
        var pOther = func.GetParam(1);
        pOther.Name = "other";

        builder.PositionAtEnd(func.AppendBasicBlock("entry"));
        var u1 = builder.BuildStructGEP2(Number, pThis, 0, "unbox1");
        var u2 = builder.BuildStructGEP2(Number, pOther, 0, "unbox2");
        var v1 = builder.BuildLoad2(ctx.Int32Type, u1, "val1");
        var v2 = builder.BuildLoad2(ctx.Int32Type, u2, "val2");
        LLVMValueRef res = builder.BuildMul(v1, v2, "mult");
        /* resptr points both to the structure and to its initial field */
        LLVMValueRef resptr = builder.BuildMalloc(Number, "resptr");
        builder.BuildStore(res, resptr);
        builder.BuildRet(resptr);

        func.VerifyFunction(LLVMVerifierFailureAction.LLVMPrintMessageAction);
    }
}
