﻿using System.Collections.Generic;
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
                var elementVisitor = new ElementVisitor();
                var raw = new List<IElement>();
                foreach (var element in context.element())
                {
                    var item = elementVisitor.VisitElement(element);
                    raw.Add(item);
                }

                var elements = new List<IElement>();
                IElement last = null;
                var indentation = IndentationTracker.Instance.Minimum;
                // IMPORTANT: block quote processing
                for (int i = 0; i < raw.Count; i++)
                {
                    var current = raw[i];
                    if (current.TypeCode == ElementType.Comment)
                    {
                        elements.Add(current);
                        last = current;
                        continue;
                    }

                    var block = last as BlockQuote;
                    if (block == null)
                    {
                        if (current.Lines[0].IsIndented)
                        {
                            var level = current.Lines[0].Indentation / indentation;
                            while (level > 0)
                            {
                                current = new BlockQuote(level, current);
                                level--;
                            }
                        }

                        elements.Add(current);
                        last = current;
                        continue;
                    }

                    if (current.Lines[0].IsIndented)
                    {
                        var level = current.Lines[0].Indentation / indentation;
                        block.Eat(current, level);
                    }
                    else
                    {
                        elements.Add(current);
                        last = current;
                    }
                }


                return new Document(elements);
            }
        }

        class ElementVisitor : ReStructuredTextBaseVisitor<IElement>
        {
            public override IElement VisitElement([NotNull] ElementContext context)
            {
                var commentContext = context.comment();
                if (commentContext != null)
                {
                    var commentVisitor = new CommentVisitor();
                    var comment = commentVisitor.VisitComment(commentContext);
                    return comment;
                }

                var paragraphVisitor = new ParagraphVisitor();
                var paragraph = paragraphVisitor.VisitParagraph(context.paragraph());
                return paragraph;
            }
        }

        class CommentVisitor : ReStructuredTextBaseVisitor<Comment>
        {
            public override Comment VisitComment([NotNull] CommentContext context)
            {
                var lineVisitor = new LineVisitor();
                var lines = new List<Line>();
                foreach (var line in context.line())
                {
                    lines.Add(lineVisitor.VisitLine(line));
                }

                return new Comment(lines);
            }
        }

        class ParagraphVisitor : ReStructuredTextBaseVisitor<Paragraph>
        {
            public override Paragraph VisitParagraph([NotNull] ParagraphContext context)
            {
                var lineVisitor = new LineVisitor();
                var lines = new List<Line>();
                foreach (var line in context.line())
                {
                    lines.Add(lineVisitor.VisitLine(line));
                }

                return new Paragraph(lines);
            }
        }

        class LineVisitor : ReStructuredTextBaseVisitor<Line>
        {
            public override Line VisitLine([NotNull] LineContext context)
            {
                var indentation = context.indentation();
                var textVisitor = new TextVisitor();
                var text = context.text();
                int length = indentation == null ? 0 : indentation.GetText().Length;
                IndentationTracker.Instance.Track(length);
                return new Line(textVisitor.VisitText(text)) { IsIndented = indentation != null, Indentation = length };
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

        class IndentationTracker
        {
            public static IndentationTracker Instance = new IndentationTracker();

            public void Track(int indentation)
            {
                if (indentation == 0)
                {
                    return;
                }

                if (Minimum > 0 && Minimum < indentation)
                {
                    return;
                }

                Minimum = indentation;
            }

            public int Minimum
            {
                get; private set;
            }
        }
    }
}
