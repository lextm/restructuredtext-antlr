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
    public class Paragraph : IElement
    {
        public IList<ITextArea> TextAreas { get; }

        public Paragraph(IList<ITextArea> textAreas)
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
    }
}
