using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChineseObjects.Lang
{
    internal partial class LangParser
    {
        public LangParser() : base(null) { }

        public Program Parse(string s)
        {
            byte[] inputBuffer = System.Text.Encoding.Default.GetBytes(s);
            MemoryStream stream = new MemoryStream(inputBuffer);
            this.Scanner = new LangScanner(stream);
            Console.WriteLine(this.Parse());
            Console.WriteLine(CurrentSemanticValue.expr);
            return CurrentSemanticValue.program;
        }
    }
}
