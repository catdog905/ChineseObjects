namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree
{
    internal partial class LangScanner
    {

        public override void yyerror(string format, params object[] args)
        {
            base.yyerror(format, args);
            Console.WriteLine(base.yylval.identifier.ToString());
            Console.WriteLine(format, args);
            Console.WriteLine();
        }
    }
}
