﻿// Copyright (C) 2017 Lex Li
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

namespace Lextm.ReStructuredText
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
            return ParseContent(File.ReadAllText(fileName));
        }

        public static Document ParseContent(string text)
        {
            try
            {
                var lexer = new ReStructuredTextLexer(new AntlrInputStream('\n' + text));
                var tokens = new CommonTokenStream(lexer);
                var parser = new ReStructuredTextParser(tokens);
                DocumentVisitor visitor = new DocumentVisitor
                {
                    IndentationTracker = new IndentationTracker(),
                    SectionTracker = new SectionTracker()
                };
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
                var separator = context.SectionSeparator()[0].GetText();
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

                return new Section(level, title, list, context.overline != null);
            }
        }

        class ListItemVisitor : TrackedBaseVisitor<ListItem>
        {
            public override ListItem VisitListItemBullet([NotNull] ListItemBulletContext context)
            {
                if (context.special != null)
                {
                    var indentation = context.indentation();
                    int length = indentation == null ? 0 : indentation.GetText().Length;
                    IndentationTracker.Track(length);
                    return new ListItem(context.special.Text, null, new List<IElement>(0)) { Indentation = length };
                }

                var simple = context.bulletSimple();
                if (simple != null)
                {
                    return VisitBulletSimple(simple);
                }

                return VisitBulletCrossLine(context.bulletCrossLine());
            }

            public override ListItem VisitBulletSimple([NotNull] BulletSimpleContext context)
            {
                var indentation = context.indentation();
                int length = indentation == null ? 0 : indentation.GetText().Length;
                IndentationTracker.Track(length);
                var start = context.bullet().GetText();
                var list = new List<IElement>();
                var paragraphVisitor = new ParagraphVisitor().Inherit(this);
                var first = context.paragraphNoBreak();
                if (first != null)
                {
                    list.Add(paragraphVisitor.VisitParagraphNoBreak(first));
                }

                var paragraph = context.paragraph();
                if (paragraph != null)
                {
                    foreach (var item in paragraph)
                    {
                        list.Add(paragraphVisitor.VisitParagraph(item));
                    }
                }

                return new ListItem(start, null, list) { Indentation = length };
            }

            public override ListItem VisitBulletCrossLine([NotNull] BulletCrossLineContext context)
            {
                var indentation = context.indentation();
                int length = indentation == null ? 0 : indentation.GetText().Length;
                IndentationTracker.Track(length);
                var start = context.bullet().GetText();
                var list = new List<IElement>();
                var paragraphVisitor = new ParagraphVisitor().Inherit(this);
                var paragraph = context.paragraph();
                if (paragraph != null)
                {
                    foreach (var item in paragraph)
                    {
                        list.Add(paragraphVisitor.VisitParagraph(item));
                    }
                }

                return new ListItem(start, null, list) { Indentation = length };
            }

            public override ListItem VisitListItemEnumerated(ListItemEnumeratedContext context)
            {
                var enumerator = context.enumerated.GetText();
                var list = new List<IElement>();
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
                foreach (var line in context.lineBlockLine())
                {
                    lines.Add(new Line(lineVisitor.VisitLineBlockLine(line)));
                }

                return new LineBlock(lines);
            }
        }

        class CommentVisitor : TrackedBaseVisitor<Comment>
        {
            public override Comment VisitComment([NotNull] CommentContext context)
            {
                var result = new List<ITextArea>();
                var indentation = context.indentation();
                int length = indentation == null ? 0 : indentation.GetText().Length;
                IndentationTracker.Track(length);

                var commentLineContext = context.commentLineNoBreak();
                if (commentLineContext != null)
                {
                    var visitor = new TextAreaVisitor().Inherit(this);
                    result.Add(visitor.VisitCommentLineNoBreak(commentLineContext));
                }

                var linesContext = context.commentParagraphs();
                if (linesContext != null)
                {            
                    var lineVisitor = new TextAreasVisitor().Inherit(this);
                    
                    result.AddRange(lineVisitor.VisitCommentParagraphs(linesContext));
                }

                return new Comment(result) {Indentation = length};
            }
        }

        class ParagraphVisitor : TrackedBaseVisitor<Paragraph>
        {
            public override Paragraph VisitParagraph([NotNull] ParagraphContext context)
            {
                var lineVisitor = new TextAreasVisitor().Inherit(this);
                var lines = new List<ITextArea>();
                var children = context.lines();
                lines.AddRange(lineVisitor.VisitLines(children));
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

                var children = context.lines();
                foreach (var child in children)
                {
                    lines.AddRange(lineVisitor.VisitLines(child));
                }

                return new Paragraph(lines);
            }
        }

        class TextAreasVisitor : TrackedBaseVisitor<ITextArea[]>
        {
            public override ITextArea[] VisitCommentParagraphs([NotNull] CommentParagraphsContext context)
            {
                var result = new List<ITextArea>();
                result.AddRange(VisitCommentParagraph(context.main));
                var rest = context.commentRest();
                if (rest != null)
                {
                    result.AddRange(VisitCommentRest(rest));
                }

                return result.ToArray();
            }
            
            public override ITextArea[] VisitCommentRest([NotNull] CommentRestContext context)
            {
                var result = new List<ITextArea>();
                var rest = context.commentParagraph();
                if (rest != null)
                {
                    foreach (var item in rest)
                    {
                        result.AddRange(VisitCommentParagraph(item));
                    }
                }

                return result.ToArray();
            }

            public override ITextArea[] VisitCommentParagraph([NotNull] CommentParagraphContext context)
            {
                var lineVisitor = new TextAreaVisitor().Inherit(this);
                var result = new List<ITextArea>();
                foreach (var item in context.commentLine())
                {
                    result.Add(lineVisitor.VisitCommentLine(item));
                }
                
                return result.ToArray();
            }
            
            public override ITextArea[] VisitTitle([NotNull] TitleContext context)
            {
                var lineContext = context.lineNormal();
                if (lineContext != null)
                {
                    return VisitLineNormal(lineContext);
                }

                var lineStarContext = context.lineStar();
                if (lineStarContext != null)
                {
                    return VisitLineStar(lineStarContext);
                }

                return new ITextArea[] { new TextArea(context.GetText(), 
                    new Scope { LineStart = context.Start.Line + 1, LineEnd = context.Stop.Line }) };
            }

            public override ITextArea[] VisitLines([NotNull] LinesContext context)
            {
                var simple = context.linesNormal();
                if (simple != null)
                {
                    return VisitLinesNormal(simple);
                }

                return VisitLinesStar(context.linesStar());
            }

            public override ITextArea[] VisitLinesNormal([NotNull] LinesNormalContext context)
            {
                var result = new List<ITextArea>();
                result.AddRange(VisitLineNormal(context.lineNormal()));
                var normal = context.linesNormal();
                if (normal != null)
                {
                    result.AddRange(VisitLinesNormal(normal));
                }

                var star = context.linesStar();
                if (star != null)
                {
                    result.AddRange(VisitLinesStar(star));
                }

                return result.ToArray();
            }

            public override ITextArea[] VisitLinesStar([NotNull] LinesStarContext context)
            {
                var result = new List<ITextArea>();
                result.AddRange(VisitLineStar(context.lineStar()));

                var no = context.lineNoBreak();
                if (no != null)
                {
                    result.AddRange(VisitLineNoBreak(no));
                }

                var normal = context.linesNormal();
                if (normal != null)
                {
                    result.AddRange(VisitLinesNormal(normal));
                }

                var star = context.linesStar();
                if (star != null)
                {
                    result.AddRange(VisitLinesStar(star));
                }

                return result.ToArray();
            }

            public override ITextArea[] VisitLineNoBreak([NotNull] LineNoBreakContext context)
            {
                var result = new List<ITextArea>();
                var indentation = context.indentation();
                int length = indentation == null ? 0 : indentation.GetText().Length;
                IndentationTracker.Track(length);

                var spanVisitor = new TextAreaVisitor().Inherit(this);
                result.Add(spanVisitor.VisitSpanLineStartNoStar(context.spanLineStartNoStar()));
                var spanContext = context.span();
                if (spanContext != null)
                {
                    foreach (var atom in spanContext)
                    {
                        result.Add(spanVisitor.VisitSpan(atom));
                    }
                }

                if (result.Last().TypeCode == ElementType.Text)
                {
                    result.Last().Content.Append("\n");
                }
                else
                {
                    result.Add(new TextArea("\n", result.Last().Scope));
                }

                result.First().Indentation = length;
                return result.ToArray();
            }

            public override ITextArea[] VisitLineNormal([NotNull] LineNormalContext context)
            {
                var result = new List<ITextArea>();
                var special = context.lineSpecial();
                if (special != null)
                {
                    var text = context.GetText().TrimStart() + "\n";
                    return new ITextArea[] { new TextArea(text, new Scope { LineStart = context.Start.Line, LineEnd = context.Stop.Line }) };
                }

                var indentation = context.indentation();
                int length = indentation == null ? 0 : indentation.GetText().Length;
                IndentationTracker.Track(length);

                var spanVisitor = new TextAreaVisitor().Inherit(this);
                var start = context.spanLineStartNoStar();
                if (start != null)
                {
                    foreach (var item in start)
                    {
                        result.Add(spanVisitor.VisitSpanLineStartNoStar(item));
                    }
                }

                var bodyContext = context.span();
                foreach (var atom in bodyContext)
                {
                    result.Add(spanVisitor.VisitSpan(atom));
                }

                var noStar = context.spanNoStar();
                if (noStar != null)
                {
                    foreach (var item in noStar)
                    {
                        result.Add(spanVisitor.VisitSpanNoStar(item));
                    }
                }

                if (result.Last().TypeCode == ElementType.Text)
                {
                    result.Last().Content.Append("\n");
                }
                else
                {
                    result.Add(new TextArea("\n", result.Last().Scope));
                }

                result.First().Indentation = length;
                return result.ToArray();
            }

            public override ITextArea[] VisitLineStar([NotNull] LineStarContext context)
            {
                var result = new List<ITextArea>();
                var indentation = context.indentation();
                int length = indentation == null ? 0 : indentation.GetText().Length;
                IndentationTracker.Track(length);

                var spanVisitor = new TextAreaVisitor().Inherit(this);
                var bodyContext = context.spanLineStartNoStar();
                foreach (var atom in bodyContext)
                {
                    result.Add(spanVisitor.VisitSpanLineStartNoStar(atom));
                }

                var starText = context.starText();
                if (starText != null)
                {
                    result.Add(spanVisitor.VisitStarText(starText));
                }

                result.First().Indentation = length;
                return result.ToArray();
            }

            public override ITextArea[] VisitLineBlockLine([NotNull] LineBlockLineContext context)
            {
                var indentation = context.indentation();
                var result = new List<ITextArea>();
                int length = indentation == null ? 0 : indentation.GetText().Length;
                IndentationTracker.Track(length);

                var spanVisitor = new TextAreaVisitor().Inherit(this);
                var bodyContext = context.span();
                foreach (var atom in bodyContext)
                {
                    result.Add(spanVisitor.VisitSpan(atom));
                }

                var starTextContext = context.starText();
                if (starTextContext != null)
                {
                    result.Add(spanVisitor.VisitStarText(starTextContext));
                }

                result.First().Indentation = length;
                return result.ToArray();
            }
        }

        class TextAreaVisitor : TrackedBaseVisitor<ITextArea>
        {
            public override ITextArea VisitCommentLine([NotNull] CommentLineContext context)
            {
                return VisitCommentLineNoBreak(context.commentLineNoBreak());
            }
            
            public override ITextArea VisitCommentLineNoBreak([NotNull] CommentLineNoBreakContext context)
            {
                return new TextArea(context.commentLineAtoms().GetText() + "\n", new Scope { LineStart = context.Start.Line, LineEnd = context.Stop.Line });
            }
            
            public override ITextArea VisitText([NotNull] TextContext context)
            {
                return new TextArea(context.GetText(), new Scope { LineStart = context.Start.Line, LineEnd = context.Stop.Line });
            }

            public override ITextArea VisitTextLineStart([NotNull] TextLineStartContext context)
            {
                return new TextArea(context.GetText(), new Scope { LineStart = context.Start.Line, LineEnd = context.Stop.Line });
            }

            public override ITextArea VisitSpan([NotNull] SpanContext context)
            {
                var star = context.starText();
                if (star != null)
                {
                    return VisitStarText(star);
                }

                return VisitSpanNoStar(context.spanNoStar());
            }

            public override ITextArea VisitSpanNoStar([NotNull] SpanNoStarContext context)
            {
                var inline = context.backTickText();
                if (inline != null)
                {
                    return VisitBackTickText(inline);
                }

                var text = context.text();
                if (text != null)
                {
                    return VisitText(text);
                }

                // TODO:
                return new TextArea(context.GetText(), new Scope { LineStart = context.Start.Line, LineEnd = context.Stop.Line });
            }

            public override ITextArea VisitSpanLineStartNoStar([NotNull] SpanLineStartNoStarContext context)
            {
                var inline = context.backTickText();
                if (inline != null)
                {
                    return VisitBackTickText(inline);
                }

                var text = context.textLineStart();
                if (text != null)
                {
                    return VisitTextLineStart(text);
                }

                // TODO:
                return new TextArea(context.GetText(), new Scope { LineStart = context.Start.Line, LineEnd = context.Stop.Line });
            }

            public override ITextArea VisitBackTickText([NotNull] BackTickTextContext context)
            {
                return new BackTickText(
                    context.titled == null ? null : context.titled.Text,
                    context.body().GetText(),
                    new Scope { LineStart = context.body().Start.Line, LineEnd = context.body().Stop.Line });
            }

            public override ITextArea VisitStarText([NotNull] StarTextContext context)
            {
                return new StarText(context.GetText(), new Scope { LineStart = context.Start.Line, LineEnd = context.Stop.Line });
            }
        }

        class TrackedBaseVisitor<T> : ReStructuredTextBaseVisitor<T>, ITracked
        {
            public SectionTracker SectionTracker { get; set; }

            public IndentationTracker IndentationTracker { get; set; }

            internal TrackedBaseVisitor<T> Inherit(ITracked item)
            {
                SectionTracker = item.SectionTracker;
                IndentationTracker = item.IndentationTracker;
                return this;
            }
        }
    }

    interface ITracked
    {
        SectionTracker SectionTracker { get; }
        IndentationTracker IndentationTracker { get; }
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
