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

namespace ReStructuredText
{
    public class ListItem : IElement
    {
        public IList<Paragraph> Elements { get; }

        public ListItem(string start, string enumerator, IList<Paragraph> element)
        {
            Start = start?[0] ?? char.MinValue;
            Enumerator = enumerator;
            if (enumerator != null)
            {
                Index = int.Parse(Enumerator.TrimEnd('.', ' '));
            }
            
            Elements = element;
        }
        
        public char Start { get; }
        public string Enumerator { get; }
        public int Index { get; }

        public ElementType TypeCode => ElementType.ListItem;
        public IList<ITextArea> TextAreas => Elements[0].TextAreas;
        public IParent Parent { get; set; }
        
        internal int LineNumber { get; set; }

        internal bool CreateNewList { get; set; } = true;

        public void Analyze(ListItem next)
        {
            if (next.LineNumber == LineNumber + 1)
            {
                if (next.Index < Index)
                {
                    CreateNewList = false;
                }
                else if (next.Index == Index + 1)
                {
                }
                else
                {
                    CreateNewList = false;
                }
            }
        }
    }
}