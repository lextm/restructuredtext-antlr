using System.Collections.Generic;
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

        public static Document ParseDocument(string fileName)
        {
            var lexer = new restructuredtextLexer(new AntlrInputStream(File.OpenRead(fileName)));
            var tokens = new CommonTokenStream(lexer);
            var parser = new restructuredtextParser(tokens);

            ReStructuredTextVisitor visitor = new ReStructuredTextVisitor();

            try
            {
                return visitor.Visit(parser.parse());
            }
            catch (RecognitionException ex)
            {
                return null;
            }
            catch (ParseCanceledException ex)
            {
                return null;
            }
        }
    }

    class ReStructuredTextVisitor : restructuredtextBaseVisitor<Document>
    {
        public override Document VisitParse([NotNull] restructuredtextParser.ParseContext context)
        {
            var paragraghVisitor = new ParagraphVisitor();
            var paragraphs = new List<Paragraph>();
            foreach (var paragraph in context.paragraph())
            {
                paragraphs.Add(paragraghVisitor.VisitParagraph(paragraph));
            }

            return new Document(paragraphs);
        }
    }

    class ParagraphVisitor : restructuredtextBaseVisitor<Paragraph>
    {
        public override Paragraph VisitParagraph([NotNull] restructuredtextParser.ParagraphContext context)
        {
            var text = context.GetText();
            return new Paragraph(text);
        }
    }

    public class Paragraph
    {
        private readonly string text;

        public Paragraph(string text)
        {
            this.text = text;
        }

        public string Text => text;
    }

    public class Document
    {
        private IList<Paragraph> paragraphs;

        public Document(IList<Paragraph> paragraphs)
        {
            this.Paragraphs = paragraphs;
        }

        public IList<Paragraph> Paragraphs { get => paragraphs; set => paragraphs = value; }
    }
}
