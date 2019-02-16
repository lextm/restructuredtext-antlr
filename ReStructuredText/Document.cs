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
    public class Document : IParent
    {
        public Document()
        {
            Elements = new List<IElement>();
        }

        public IElement Find(int line, int column)
        {
            line += 1; // IMPORTNANT: when parsing we add a \n at top.
            foreach (var item in Elements)
            {
                var result = item.Find(line, column);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public IList<IElement> Elements { get; set; }

        public ElementType TypeCode => ElementType.Document;

        public IParent Parent { get; set; }

        public int Unit { get; set; }

        public void Add(IList<ListItem> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        private IParent Add(ListItem item)
        {
            if (item.Enumerator == null)
            {
                if (!(Elements.LastOrDefault() is BulletList list) || list.Start != item.Start)
                {
                    list = new BulletList(item);
                    Add(list);
                    return item;
                }

                return list.Add(item);
            }
            else
            {
                if (!(Elements.LastOrDefault() is EnumeratedList list))
                {
                    list = new EnumeratedList(item);
                    Add(list);
                    return item;
                }

                return list.Add(item);
            }
        }

        public IParent Add(IElement element, int level = 0)
        {
            if (element is ListItem listItem)
            {
                return Add(listItem);
            }

            Elements.Add(element);
            element.Parent = this;
            return element;
        }

        internal void Eat(List<IElement> raw, ITracked tracked)
        {
            Unit = tracked.IndentationTracker.Minimum;

            IParent last = this;
            int parentIndentation = 0;
            // IMPORTANT: block quote processing
            for (int i = 0; i < raw.Count; i++)
            {
                var current = raw[i];
                if (current.TypeCode == ElementType.ListItem)
                {
                    if (current is ListItem item && item.Enumerator != null)
                    {
                        if (i < raw.Count - 1)
                        {
                            if (raw[i + 1] is ListItem next)
                            {
                                item.Analyze(next);
                            }
                        }
                    }

                    last = last.Add(current);
                    continue;
                }

                if (current.TypeCode == ElementType.Comment || current.TypeCode == ElementType.Section)
                {
                    last = last.Add(current);
                    parentIndentation = current.Indentation;
                    continue;
                }

                if (current is Paragraph paragraph)
                {
                    paragraph.Unit = Unit;
                    if (last.TypeCode == ElementType.ListItem || last.Parent?.TypeCode == ElementType.ListItem)
                    {
                        last = last.Add(current);
                        continue;
                    }

                    if (last.TypeCode == ElementType.BlockQuote)
                    {
                        if (paragraph.IsAttribution())
                        {
                            last = last.Add(paragraph);
                            continue;
                        }
                    }

                    var tuple = paragraph.Parse(parentIndentation, Unit, last);
                    if (tuple.Item1)
                    {
                        last = tuple.Item2;
                        continue;
                    }

                    current = tuple.Item2;
                }

                last = last.Add(current);
                parentIndentation = current.Indentation;
            }

            if (Elements.LastOrDefault() is BlockQuote end)
            {
                end.FillAttribution();
            }
        }

        public bool TriggerDocumentList(int line, int character)
        {
            var element = Find(line + 1, character);
            if (element == null || element.TypeCode != ElementType.Paragraph)
            {
                return false;
            }

            if (element.Parent == null)
            {
                return false;
            }

            if (element.Parent.TypeCode == ElementType.ListItem)
            {
                var text = element.TextAreas[0];
                if (text is InterpretedText interpreted)
                {
                    if (interpreted.RoleName != "doc")
                    {
                        return false;
                    }
                }
                else if (!text.Content.Text.StartsWith(":doc:", StringComparison.Ordinal))
                {
                    return false;
                }

                return true;
            }

            if (element.Parent.TypeCode == ElementType.Section && element.TextAreas.Count >= 3)
            {
                var last = element.TextAreas.Last();
                var previous = element.TextAreas[element.TextAreas.Count - 2];
                var text = element.TextAreas[element.TextAreas.Count - 3];
                if (last.Content.Text == "/\n" && previous.Content.Text == "`" && text.Content.Text.EndsWith(":doc:"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
