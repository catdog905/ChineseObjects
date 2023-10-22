using System;
using System.Collections.Generic;
using System.Text;

namespace ChineseObjects.Lang
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
