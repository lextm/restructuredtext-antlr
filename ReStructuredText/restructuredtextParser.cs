using System;
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
                var elementVisitor = new ElementVisitor();
                var raw = new List<IElement>();
                foreach (var element in context.element())
                {
                    var item = elementVisitor.VisitElement(element);
                    raw.Add(item);
                }

                var result = new Document();
                result.Eat(raw);
                return result;
            }
        }

        class ElementVisitor : ReStructuredTextBaseVisitor<IElement>
        {
            public override IElement VisitElement([NotNull] ElementContext context)
            {
                var sectionContext = context.section();
                if (sectionContext != null)
                {
                    var sectionVisitor = new SectionVisitor();
                    var section = sectionVisitor.VisitSection(sectionContext);
                    return section;
                }

                var sectionElementContext = context.sectionElement();
                if (sectionElementContext != null)
                {
                    var elementVisitor = new ElementVisitor();
                    var element = elementVisitor.VisitSectionElement(context.sectionElement());
                    return element;
                }
                
                
                //TODO: throw a better exception.
                throw new InvalidOperationException();
            }
            
            public override IElement VisitSectionElement([NotNull] SectionElementContext context)
            {
                var commentContext = context.comment();
                if (commentContext != null)
                {
                    var commentVisitor = new CommentVisitor();
                    var comment = commentVisitor.VisitComment(commentContext);
                    return comment;
                }

                var blockContext = context.lineBlock();
                if (blockContext != null)
                {
                    var blockVisitor = new LineBlockVisitor();
                    var lineBlock = blockVisitor.VisitLineBlock(blockContext);
                    return lineBlock;
                }

                var listItemContext = context.listItem();
                if (listItemContext != null)
                {
                    var listItemVisitor = new ListItemVisitor();
                    var listItem = listItemVisitor.VisitListItem(listItemContext);
                    return listItem;
                }

                var paragraphVisitor = new ParagraphVisitor();
                var paragraph = paragraphVisitor.VisitParagraph(context.paragraph());
                return paragraph;
            }
        }

        class SectionVisitor : ReStructuredTextBaseVisitor<Section>
        {
            public override Section VisitSection(SectionContext context)
            {
                var title = context.line().GetText();
                var separator = context.Section()[0].GetText();
                var level = SectionTracker.Instance.Track(separator[0]);
                var list = new List<IElement>();
                var elementVisitor = new ElementVisitor();
                var element = context.sectionElement();
                if (element != null)
                {
                    foreach (var item in element)
                    {
                        list.Add(elementVisitor.VisitSectionElement(item));
                    }
                }

                return new Section(level, title, list);
            }
            
            class SectionTracker
            {
                public static readonly SectionTracker Instance = new SectionTracker();
                private readonly IList<char> _sections = new List<char>();

                public int Track(char item)
                {
                    if (!_sections.Contains(item))
                    {
                        _sections.Add(item);
                    }

                    return _sections.IndexOf(item) + 1;
                }
            }
        }
        
        class ListItemVisitor : ReStructuredTextBaseVisitor<ListItem>
        {
            public override ListItem VisitListItem(ListItemContext context)
            {
                var start = context.Bullet().GetText();
                var list = new List<Paragraph>();
                var paragraphVisitor = new ParagraphVisitor();
                var paragraph = context.paragraph();
                if (paragraph != null)
                {
                    foreach (var item in paragraph)
                    {
                        list.Add(paragraphVisitor.VisitParagraph(item));
                    }
                }

                return new ListItem(start[0], list);
            }
        }
        
        class LineBlockVisitor : ReStructuredTextBaseVisitor<LineBlock>
        {
            public override LineBlock VisitLineBlock(LineBlockContext context)
            {
                var lineVisitor = new LineVisitor();
                var lines = new List<Line>();
                foreach (var line in context.line())
                {
                    lines.Add(lineVisitor.VisitLine(line));
                }
                
                return new LineBlock(lines);
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
                Document.IndentationTracker.Instance.Track(length);
                return new Line(textVisitor.VisitText(text)) { Indentation = length };
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
