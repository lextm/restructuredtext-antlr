using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;

namespace ReStructuredText
{
    public partial class restructuredtextParser 
    {
        public ParserRuleContext Parse()
        {
            ErrorHandler = new BailErrorStrategy();
            Interpreter.PredictionMode = PredictionMode.Sll;
            var document = parse();
            return document;
        }

        public static void ParseDocument(string fileName)
        {
            var lexer = new restructuredtextLexer(new AntlrInputStream(File.OpenRead(fileName)));
            var tokens = new CommonTokenStream(lexer);
            var parser = new restructuredtextParser(tokens);

            try
            {
                var doc = parser.Parse();
            }
            catch (RecognitionException ex)
            {

            }
            catch (ParseCanceledException ex)
            {

            }
        }
    }


}
