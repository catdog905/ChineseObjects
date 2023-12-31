using System.Text;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree
{
    internal partial class LangParser
    {
        public LangParser() : base(null) { }

        public AbstractSyntaxTree.DeclarationAwareTree.Declaration.Program Parse(string s)
        {
            byte[] inputBuffer = Encoding.Default.GetBytes(s);
            MemoryStream stream = new MemoryStream(inputBuffer);
            this.Scanner = new LangScanner(stream);
            Parse();
            return CurrentSemanticValue.program;
        }
    }
}
