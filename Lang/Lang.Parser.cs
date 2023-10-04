using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChineseObjects.Lang
{
    internal partial class LangParser
    {
        public LangParser() : base(null) { }

        public void Parse(string s)
        {
            byte[] inputBuffer = System.Text.Encoding.Default.GetBytes(s);
            MemoryStream stream = new MemoryStream(inputBuffer);
            this.Scanner = new LangScanner(stream);
            this.Parse();
        }
    }
}