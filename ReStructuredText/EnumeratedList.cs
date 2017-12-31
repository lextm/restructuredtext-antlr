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
    public class EnumeratedList : IElement
    {
        public IList<ListItem> Items { get; }
        
        public EnumeratedList(ListItem item)
        {
            Items = new List<ListItem>{item};
            item.Parent = this;
        }

        public ElementType TypeCode => ElementType.EnumeratedList;
        public IList<ITextArea> TextAreas => Items[0].TextAreas;
        public IParent Parent { get; set; }

        public IParent Add(IElement current, int level = 0)
        {
            if (current is ListItem item)
            {
                if (item.Index == Items.Last().Index + 1)
                {
                    Items.Add(item);
                    item.Parent = this;
                    return item;
                }
                
                if (item.CreateNewList)
                {
                    var list = new EnumeratedList(item);
                    Parent.Add(list);
                    return item;
                }

                // downgrade to paragraph.
                current = new Paragraph(item.TextAreas);
            }

            return Parent.Add(current);
        }

        public int Indentation => Items[0].Indentation;

        public IElement Find(int line, int column)
        {
            foreach (var item in Items)
            {
                var result = item.Find(line, column);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}