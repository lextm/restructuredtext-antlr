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

        public IList<IElement> Elements { get; }

        public ElementType TypeCode => ElementType.Document;

        public void Add(IList<ListItem> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        private void Add(ListItem item)
        {
            if (item.Enumerator == null)
            {
                if (!(Elements.LastOrDefault() is BulletList list) || list.Start != item.Start)
                {
                    list = new BulletList(item);
                    Add(list);
                }
                else
                {
                    list.Items.Add(item);
                }
            }
            else
            {
                if (!(Elements.LastOrDefault() is EnumeratedList list))
                {
                    list = new EnumeratedList(item);
                    Add(list);
                }
                else if (item.Index == list.Items.Last().Index + 1)
                {
                    list.Items.Add(item);
                }
                else if (item.CreateNewList)
                {
                    list = new EnumeratedList(item);
                    Add(list);
                }
                else
                {
                    Add(new Paragraph(item.TextAreas));
                }
            }
        }

        public void Add(IElement element, int level = 0)
        {
            if (element is ListItem listItem)
            {
                Add(listItem);
                return;
            }

            Elements.Add(element);
            element.Parent = this;
        }

        internal void Eat(List<IElement> raw, ITracked tracked)
        {
            var indentation = tracked.IndentationTracker.Minimum;

            // IMPORTANT: block quote processing
            for (int i = 0; i < raw.Count; i++)
            {
                IParent last = Elements.LastOrDefault();
                last = last ?? this;
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

                    last.Add(current);
                    continue;
                }

                if (current.TypeCode == ElementType.Comment || current.TypeCode == ElementType.Section)
                {
                    last.Add(current);
                    continue;
                }

                if (!(last is BlockQuote block))
                {
                    if (current is Paragraph paragraph)
                    {
                        var definitionList = last as DefinitionList;
                        var definitionListItems = DefinitionListItem.Parse(paragraph);
                        if (definitionListItems.Count > 0)
                        {
                            if (definitionList != null)
                            {
                                if (definitionList.Indentation == definitionListItems[0].Term.TextAreas[0].Indentation)
                                {
                                    foreach (var item in definitionListItems)
                                    {
                                        definitionList.Add(item);
                                    }
                                }
                                else
                                {
                                    var sublist = definitionList.Items[0].Definition.Elements.Last() as DefinitionList;
                                    if (sublist == null)
                                    {
                                        sublist = new DefinitionList(definitionListItems);
                                        definitionList.Items[0].Definition.Elements.Add(sublist);
                                    }
                                    else
                                    {
                                        foreach (var item in definitionListItems)
                                        {
                                            sublist.Add(item);
                                        }
                                    }
                                }

                                continue;
                            }

                            definitionList = new DefinitionList(definitionListItems);
                            current = definitionList;
                        }
                        else
                        {
                            if (current.TextAreas[0].IsIndented || current.TextAreas[0].IsQuoted)
                            {
                                var lastText = Elements.LastOrDefault()?.TextAreas?.LastOrDefault()?.Content.Text.TrimEnd();
                                if (lastText != null && lastText.EndsWith("::"))
                                {
                                    current = new LiteralBlock(current.TextAreas);
                                    Elements.Last().TextAreas.Last().Content.RemoveLiteral();
                                }
                                else
                                {
                                    if (Elements.LastOrDefault() is BulletList bullet)
                                    {
                                        if (current.TextAreas[0].Indentation == 2)
                                        {
                                            bullet.Add(current);
                                            continue;
                                        }
                                    }

                                    if (Elements.LastOrDefault() is EnumeratedList list)
                                    {
                                        if (current.TextAreas[0].Indentation == 3)
                                        {
                                            list.Add(current);
                                            continue;
                                        }
                                    }

                                    var level = current.TextAreas[0].Indentation / indentation;
                                    while (level > 0)
                                    {
                                        current = new BlockQuote(level, current);
                                        level--;
                                    }
                                }
                            }
                        }
                    }

                    last.Add(current);
                    continue;
                }

                if (current.TextAreas[0].IsIndented)
                {
                    var level = current.TextAreas[0].Indentation / indentation;
                    block.Add(current, level);
                }
                else
                {
                    last.Add(current);
                }
            }
        }
    }
}
