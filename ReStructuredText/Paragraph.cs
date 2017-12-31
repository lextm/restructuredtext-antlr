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
using System.Linq;

namespace Lextm.ReStructuredText
{
    public class Paragraph : IElement
    {
        public IList<ITextArea> TextAreas { get; }

        public int Unit { get; set; }

        public Paragraph(IEnumerable<ITextArea> textAreas)
        {
            TextAreas = new List<ITextArea>();
            foreach (var area in textAreas)
            {
                if (area.TypeCode == ElementType.BackTickText)
                {
                    ((BackTickText)area).Process(TextAreas);
                }
                else if (area.TypeCode == ElementType.StarText)
                {
                    ((StarText)area).Process(TextAreas);
                }
                else
                {
                    TextAreas.Add(area);
                }
            }

            StarText.Deemphasize(TextAreas);
        }

        public bool IsBlockQuote => TextAreas.Count > 0 && TextAreas[0].IsIndented;

        public ElementType TypeCode => ElementType.Paragraph;

        public IParent Parent { get; set; }

        public override string ToString()
        {
            return TextAreas[0].Content.Text;
        }

        public IElement Find(int line, int column)
        {
            var first = TextAreas.First();
            var last = TextAreas.Last();
            if (line < first.Scope.LineStart || line > last.Scope.LineEnd)
            {
                return null;
            }

            return this;
        }

        public IParent Add(IElement element, int level = 0)
        {
            return Parent.Add(element);
        }

        public int Indentation => TextAreas[0].Indentation;

        public bool IsAttribution()
        {
            var text = TextAreas[0].Content.Text;
            if (!text.StartsWith("--") && !text.StartsWith("\u2014"))
            {
                return false;
            }
            
            int? indent = null;
            for (var i = 0; i < TextAreas.Count; i++)
            {
                var info = TextAreas[i];
                if (info.Content.Text.Last() == '\n')
                {
                    var next = i + 1;
                    if (next == TextAreas.Count)
                    {
                        continue;
                    }

                    var indentation = TextAreas[next].Indentation;
                    if (indent == null)
                    {
                        indent = indentation;
                    }
                    else
                    {
                        if (indentation != indent)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public Tuple<bool, IElement> Parse(int parentIndentation, int unit, IParent last)
        {
            var definitionListItems = DefinitionListItem.Parse(this, last);
            if (definitionListItems.Count > 0)
            {
                var definitionList = last.Parent as DefinitionList;
                if (last.Parent is DefinitionListItem)
                {
                    definitionList = (DefinitionList) last.Parent.Parent;
                }

                if (definitionList != null)
                {
                    foreach (var item in definitionListItems)
                    {
                        definitionList.Add(item);
                    }

                    return new Tuple<bool, IElement>(true, definitionListItems.Last());
                }

                definitionList = new DefinitionList(definitionListItems);
                definitionList.Parent = last;
                last.Add(definitionList);
                var listItem = definitionListItems.Last();
                return new Tuple<bool, IElement>(true, listItem);
            }

            IElement newElement = this;
            if (Indentation > parentIndentation || TextAreas[0].IsQuoted)
            {
                var lastElement = last as IElement;
                var lastText = lastElement?.TextAreas?.LastOrDefault()?.Content.Text.TrimEnd();
                if (lastText != null && lastText.EndsWith("::"))
                {
                    newElement = new LiteralBlock(TextAreas);
                    lastElement?.TextAreas.Last().Content.RemoveLiteral();
                }
                else
                {
                    var level = newElement.TextAreas[0].Indentation / unit;
                    while (level > 0)
                    {
                        newElement = new BlockQuote(level * unit, unit, newElement);
                        level--;
                    }
                }
            }

            return new Tuple<bool, IElement>(false, newElement);
        }
    }
}
