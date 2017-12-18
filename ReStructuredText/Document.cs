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

namespace ReStructuredText
{
    public class Document : IParent
    {
        public Document()
        {
            Elements = new List<IElement>();
        }

        public IList<IElement> Elements { get; }

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
                    Elements.Add(list);
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
                    Elements.Add(list);
                }
                else if (item.Index == list.Items.Last().Index + 1)
                {
                    list.Items.Add(item);
                }
                else if (item.HasEnding)
                {
                    list = new EnumeratedList(item);
                    Elements.Add(list);
                }
                else
                {
                    Elements.Add(new Paragraph(item.TextAreas));
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
            Section section = null;
            // IMPORTANT: block quote processing
            for (int i = 0; i < raw.Count; i++)
            {
                var current = raw[i];
                if (current.TypeCode == ElementType.Comment || current.TypeCode == ElementType.ListItem)
                {
                    if (section == null)
                    {
                        Add(current);
                    }
                    else
                    {
                        section.Add(current);
                    }

                    continue;
                }

                if (current.TypeCode == ElementType.Section)
                {
                    var newSection = current as Section;  
                    if (section == null)
                    {
                        Add(current);
                    }
                    else
                    {
                        section.Add(current);
                    }
                    
                    section = newSection ?? section;
                    continue;
                }

                if (!(Elements.LastOrDefault() is BlockQuote block))
                {
                    if (current.TextAreas[0].IsIndented || current.TextAreas[0].IsQuoted)
                    {
                        var last = Elements.LastOrDefault()?.TextAreas?.LastOrDefault()?.Content.Text.TrimEnd();
                        if (last != null && last.EndsWith("::"))
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

                    if (section == null)
                    {
                        Add(current);
                    }
                    else
                    {
                        section.Add(current);
                    }

                    continue;
                }

                if (current.TextAreas[0].IsIndented)
                {
                    var level = current.TextAreas[0].Indentation / indentation;
                    block.Add(current, level);
                }
                else
                {
                    if (section == null)
                    {
                        Add(current);
                    }
                    else
                    {
                        section.Add(current);
                    }
                }
            }
        }
    }
}
