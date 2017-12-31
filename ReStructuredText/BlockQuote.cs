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
    public class BlockQuote : IElement
    {
        private readonly int _unit;

        public BlockQuote(int indentation, int unit, params IElement[] content)
        {
            _unit = unit;
            Indentation = indentation;
            Elements = new List<IElement>(content);
            foreach (var item in Elements)
            {
                item.Parent = this;
            }
        }

        public Attribution Attribution { get; private set; }

        public int Indentation { get; }

        public IList<IElement> Elements { get; private set; }

        public ElementType TypeCode => ElementType.BlockQuote;

        // TODO:
        public IList<ITextArea> TextAreas => Elements[0].TextAreas;

        public IParent Parent { get; set; }

        public int Level => Indentation / _unit;

        public IParent Add(IElement current, int level = 0)
        {
            if (current is BlockQuote block)
            {
                var quoteLevel = block.Level;
                if (quoteLevel == Level)
                {
                    foreach (var item in block.Elements)
                    {
                        Elements.Add(item);
                        item.Parent = this;
                    }

                    return this;
                }

                if (quoteLevel < Level)
                {
                    return Parent.Add(current, quoteLevel);
                }

                Elements.Add(block);
                block.Parent = this;
                return this;
            }

            if (current.Indentation >= Indentation)
            {
                Elements.Add(current);
                return this;
            }

            FillAttribution();
            return Parent.Add(current);
        }

        public IElement Find(int line, int column)
        {
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

        internal void FillAttribution()
        {
            for (var index = 1; index < Elements.Count; index++)
            {
                var item = Elements[index];
                if (item is Paragraph last)
                {
                    var text = last.TextAreas[0].Content.Text;
                    if (text.StartsWith("--") || text.StartsWith("\u2014"))
                    {
                        int line = 0;
                        int? indent = null;
                        bool isAttribution = true;
                        for (var i = 0; i < last.TextAreas.Count; i++)
                        {
                            var info = last.TextAreas[i];
                            if (info.Content.Text.Last() == '\n')
                            {
                                if (line > 0)
                                {
                                    var next = i + 1;
                                    if (next == last.TextAreas.Count)
                                    {
                                        continue;
                                    }

                                    var indentation = last.TextAreas[next].Indentation;
                                    if (indent == null)
                                    {
                                        indent = indentation;
                                    }
                                    else
                                    {
                                        if (indentation != indent)
                                        {
                                            isAttribution = false;
                                            break;
                                        }
                                    }
                                }

                                line++;
                            }
                        }

                        if (isAttribution)
                        {
                            Attribution = new Attribution(last.TextAreas);
                            if (index < Elements.Count - 1)
                            {
                                var newItem = new BlockQuote(Indentation, _unit, Elements.Skip(index + 1).ToArray());
                                newItem.FillAttribution();
                                Parent.Add(newItem);
                                Elements = Elements.Take(index + 1).ToList();
                            }

                            Elements.Remove(last);
                        }
                    }
                }
            }
        }
    }
}
