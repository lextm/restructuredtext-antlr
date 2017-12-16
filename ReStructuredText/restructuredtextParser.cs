using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;

namespace ReStructuredText
{
    public partial class ReStructuredTextParser
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
            var lexer = new ReStructuredTextLexer(new AntlrInputStream(File.OpenRead(fileName)));
            var tokens = new CommonTokenStream(lexer);
            var parser = new ReStructuredTextParser(tokens);

            DocumentVisitor visitor = new DocumentVisitor();

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

        class DocumentVisitor : ReStructuredTextBaseVisitor<Document>
        {
            public override Document VisitParse([NotNull] ParseContext context)
            {
                var paragraghVisitor = new ParagraphVisitor();
                var paragraphs = new List<Paragraph>();
                foreach (var paragraph in context.paragraph())
                {
                    var item = paragraghVisitor.VisitParagraph(paragraph);
                    paragraphs.Add(item);
                }

                return new Document(paragraphs);
            }
        }

        class ParagraphVisitor : ReStructuredTextBaseVisitor<Paragraph>
        {
            public override Paragraph VisitParagraph([NotNull] ParagraphContext context)
            {
                var comment = context.Comment();
                var lineVisitor = new LineVisitor();
                var lines = new List<Line>();
                foreach (var line in context.line())
                {
                    lines.Add(lineVisitor.VisitLine(line));
                }

                return new Paragraph(lines) { IsComment = comment != null };
            }
        }

        class LineVisitor : ReStructuredTextBaseVisitor<Line>
        {
            public override Line VisitLine([NotNull] LineContext context)
            {
                var indentation = context.indentation();
                var textVisitor = new TextVisitor();
                var text = context.text();
                return new Line(textVisitor.VisitText(text)) { IsIndented = indentation != null };
            }
        }

        class TextVisitor : ReStructuredTextBaseVisitor<Text>
        {
            public override Text VisitText([NotNull] TextContext context)
            {
                var text = context.GetText();
                return new Text(text);
            }
        }
    }
}
