using System.Text;

namespace ChineseObjects.Lang
{
    internal partial class LangParser
    {
        public LangParser() : base(null) { }

        public AbstractSyntaxTree.DeclarationAwareTree.Declaration.Program Parse(string s)
        {
            byte[] inputBuffer = Encoding.Default.GetBytes(s);
            MemoryStream stream = new MemoryStream(inputBuffer);
            this.Scanner = new LangScanner(stream);
            Console.WriteLine(this.Parse());
            return CurrentSemanticValue.program;
        }
    }
}
