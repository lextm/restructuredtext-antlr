// Copyright (C) 2017 Lex Li
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;

namespace ReStructuredText
{
    public partial class ReStructuredTextParser
    {
        private SectionTracker _sectionTrackerInstance = new SectionTracker();
        
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

            DocumentVisitor visitor = new DocumentVisitor()
            {
                IndentationTracker = new IndentationTracker(),
                SectionTracker = new SectionTracker()
            };

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

        class DocumentVisitor : TrackedBaseVisitor<Document>
        {
            public override Document VisitParse([NotNull] ParseContext context)
            {
                var elementVisitor = new ElementVisitor().Inherit(this);
                var raw = new List<IElement>();
                foreach (var element in context.element())
                {
                    var item = elementVisitor.VisitElement(element);
                    raw.Add(item);
                }

                var result = new Document();
                result.Eat(raw, this);
                return result;
            }
        }

        class ElementVisitor : TrackedBaseVisitor<IElement>
        {
            public override IElement VisitElement([NotNull] ElementContext context)
            {
                var sectionContext = context.section();
                if (sectionContext != null)
                {
                    var sectionVisitor = new SectionVisitor().Inherit(this);
                    var section = sectionVisitor.VisitSection(sectionContext);
                    return section;
                }

                var sectionElementContext = context.sectionElement();
                if (sectionElementContext != null)
                {
                    var elementVisitor = new ElementVisitor().Inherit(this);
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
                    var commentVisitor = new CommentVisitor().Inherit(this);
                    var comment = commentVisitor.VisitComment(commentContext);
                    return comment;
                }

                var blockContext = context.lineBlock();
                if (blockContext != null)
                {
                    var blockVisitor = new LineBlockVisitor().Inherit(this);
                    var lineBlock = blockVisitor.VisitLineBlock(blockContext);
                    return lineBlock;
                }

                var listItemContext = context.listItemBullet();
                if (listItemContext != null)
                {
                    var listItemVisitor = new ListItemVisitor().Inherit(this);
                    var listItem = listItemVisitor.VisitListItemBullet(listItemContext);
                    return listItem;
                }
                
                var listItemContext2 = context.listItemEnumerated();
                if (listItemContext2 != null)
                {
                    var listItemVisitor = new ListItemVisitor().Inherit(this);
                    var listItem = listItemVisitor.VisitListItemEnumerated(listItemContext2);
                    return listItem;
                }

                var paragraphVisitor = new ParagraphVisitor().Inherit(this);
                var paragraph = paragraphVisitor.VisitParagraph(context.paragraph());
                return paragraph;
            }
        }

        class SectionVisitor : TrackedBaseVisitor<Section>
        {
            public override Section VisitSection(SectionContext context)
            {
                var titleVisitor = new TextAreasVisitor().Inherit(this);
                var title = titleVisitor.VisitTitle(context.title());
                var separator = context.Section()[0].GetText();
                var level = SectionTracker.Track(separator[0]);
                var list = new List<IElement>();
                var elementVisitor = new ElementVisitor().Inherit(this);
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
        }
        
        class ListItemVisitor : TrackedBaseVisitor<ListItem>
        {
            public override ListItem VisitListItemBullet(ListItemBulletContext context)
            {
                if (context.special != null)
                {
                    return new ListItem(context.special.Text, null, new List<Paragraph>(0));
                }

                var start = context.bullet().GetText();
                var list = new List<Paragraph>();
                var paragraphVisitor = new ParagraphVisitor().Inherit(this);
                var paragraph = context.paragraph();
                if (paragraph != null)
                {
                    foreach (var item in paragraph)
                    {
                        list.Add(paragraphVisitor.VisitParagraph(item));
                    }
                }

                return new ListItem(start, null, list);
            }
            
            public override ListItem VisitListItemEnumerated(ListItemEnumeratedContext context)
            {
                var enumerator = context.enumerated.GetText();
                var list = new List<Paragraph>();
                var paragraphVisitor = new ParagraphVisitor().Inherit(this);
                var start = context.paragraphNoBreak();
                if (start != null)
                {
                    list.Add(paragraphVisitor.VisitParagraphNoBreak(start));
                }
                var paragraph = context.paragraph();
                if (paragraph != null)
                {
                    foreach (var item in paragraph)
                    {
                        list.Add(paragraphVisitor.VisitParagraph(item));
                    }
                }

                return new ListItem(null, enumerator, list) { LineNumber = context.LineBreak().Symbol.Line };
            }
        }
        
        class LineBlockVisitor : TrackedBaseVisitor<LineBlock>
        {
            public override LineBlock VisitLineBlock(LineBlockContext context)
            {
                var lineVisitor = new TextAreasVisitor().Inherit(this);
                var lines = new List<Line>();
                foreach (var line in context.lineBlockAtom())
                {
                    lines.Add(new Line(lineVisitor.VisitLineBlockAtom(line)));
                }
                
                return new LineBlock(lines);
            }
        }

        class CommentVisitor : TrackedBaseVisitor<Comment>
        {
            public override Comment VisitComment([NotNull] CommentContext context)
            {
                InComment = true;
                var lines = new List<ITextArea>();
                var lineVisitor = new TextAreasVisitor().Inherit(this);
                var commentLineContext = context.lineNoBreak();
                if (commentLineContext != null)
                {
                    lines.AddRange(lineVisitor.VisitLineNoBreak(commentLineContext));
                }

                var lineContext = context.line();
                if (lineContext != null)
                {
                    foreach (var line in lineContext)
                    {
                        lines.AddRange(lineVisitor.VisitLine(line));
                    }
                }

                InComment = false;

                return new Comment(lines);
            }
        }

        class ParagraphVisitor : TrackedBaseVisitor<Paragraph>
        {
            public override Paragraph VisitParagraph([NotNull] ParagraphContext context)
            {
                var lineVisitor = new TextAreasVisitor().Inherit(this);
                var lines = new List<ITextArea>();
                var children = context.line();
                foreach (var child in children)
                {
                    lines.AddRange(lineVisitor.VisitLine(child));
                }

                return new Paragraph(lines);
            }

            public override Paragraph VisitParagraphNoBreak([NotNull] ParagraphNoBreakContext context)
            {
                var lineVisitor = new TextAreasVisitor().Inherit(this);
                var lines = new List<ITextArea>();
                var noBreak = context.lineNoBreak();
                if (noBreak != null)
                {
                    lines.AddRange(lineVisitor.VisitLineNoBreak(noBreak));
                }

                var children = context.line();
                foreach (var child in children)
                {
                    lines.AddRange(lineVisitor.VisitLine(child));
                }

                return new Paragraph(lines);
            }
        }

        class TextAreasVisitor : TrackedBaseVisitor<ITextArea[]>
        {
            public override ITextArea[] VisitTitle([NotNull] TitleContext context)
            {
                var lineContext = context.line();
                if (lineContext != null)
                {
                    return VisitLine(lineContext);
                }

                return new ITextArea[] { new TextArea(context.GetText()) };
            }

            public override ITextArea[] VisitLineNoBreak([NotNull] LineNoBreakContext context)
            {
                var result = new List<ITextArea>();
                var bodyContext = context.lineAtom();
                foreach (var atom in bodyContext)
                {
                    result.AddRange(VisitLineAtom(atom));
                }

                if (result.Last().TypeCode == ElementType.Text)
                {
                    result.Last().Content.Append("\n");
                }
                else
                {
                    result.Add(new TextArea("\n"));
                }

                return result.ToArray();
            }

            public override ITextArea[] VisitLine([NotNull] LineContext context)
            {
                if (InComment)
                {
                    var text = context.GetText().TrimStart() + "\n";
                    return new ITextArea[] {new TextArea(text)};
                }

                var result = new List<ITextArea>();
                var special = context.lineSpecial();
                if (special != null)
                {
                    var text = context.GetText().TrimStart() + "\n";
                    return new ITextArea[] {new TextArea(text)};
                }
                
                var indentation = context.indentation();
                int length = indentation == null ? 0 : indentation.GetText().Length;
                IndentationTracker.Track(length);

                var bodyContext = context.lineAtom();
                foreach (var atom in bodyContext)
                {
                    result.AddRange(VisitLineAtom(atom));
                }

                result.First().Indentation = length;
                if (result.Last().TypeCode == ElementType.Text)
                {
                    result.Last().Content.Append("\n");
                }
                else
                {
                    result.Add(new TextArea("\n"));
                }

                return result.ToArray();
            }
            
            public override ITextArea[] VisitLineBlockAtom([NotNull] LineBlockAtomContext context)
            {
                var indentation = context.indentation();
                var result = new List<ITextArea>();
                int length = indentation == null ? 0 : indentation.GetText().Length;
                IndentationTracker.Track(length);

                var bodyContext = context.lineAtom();
                foreach (var atom in bodyContext)
                {
                    result.AddRange(VisitLineAtom(atom));
                }

                result.First().Indentation = length;
                return result.ToArray();
            }

            
            public override ITextArea[] VisitLineAtom([NotNull] LineAtomContext context)
            {
                var span = context.span();
                if (span != null)
                {
                    var spanVisitor = new TextAreaVisitor().Inherit(this);
                    var area = spanVisitor.VisitSpan(span);
                    return new ITextArea[] {area};
                }

                var textVisitor = new TextAreaVisitor().Inherit(this);
                var text = context.text();
                return new ITextArea[]
                {
                    textVisitor.VisitText(text)
                };
            }
        }

        class TextAreaVisitor : TrackedBaseVisitor<ITextArea>
        {
            public override ITextArea VisitText([NotNull] TextContext context)
            {
                return new TextArea(context.GetText());
            }
            
            public override ITextArea VisitSpan([NotNull] SpanContext context)
            {
                var inline = context.backTickText();
                if (inline != null)
                {
                    return VisitBackTickText(inline);
                }

                var star = context.starText();
                if (star != null)
                {
                    return VisitStarText(star);
                }

                // TODO:
                return new TextArea(context.GetText());
            }

            public override ITextArea VisitBackTickText([NotNull] BackTickTextContext context)
            {
                return new BackTickText(context.titled == null ? null : context.titled.Text, new TextArea(context.body().GetText()));
            }

            public override ITextArea VisitStarText([NotNull] StarTextContext context)
            {
                return new StarText(new TextArea(context.GetText()));
            }
            
            public override ITextArea VisitTextStart([NotNull] TextStartContext context)
            {
                return new TextArea(context.GetText());
            }
            
            public override ITextArea VisitTextEnd([NotNull] TextEndContext context)
            {
                return new TextArea(context.GetText());
            }
        }

        class TrackedBaseVisitor<T> : ReStructuredTextBaseVisitor<T>, ITracked
        {
            public SectionTracker SectionTracker { get; set; }

            public IndentationTracker IndentationTracker { get; set; }
            
            public bool InComment { get; set; }

            internal TrackedBaseVisitor<T> Inherit(ITracked item)
            {
                SectionTracker = item.SectionTracker;
                IndentationTracker = item.IndentationTracker;
                InComment = item.InComment;
                return this;
            }
        }
    }

    interface ITracked
    {
        SectionTracker SectionTracker { get; }
        IndentationTracker IndentationTracker { get; }
        bool InComment { get; set; }
    }

    class SectionTracker
    {
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

    class IndentationTracker
    {
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
