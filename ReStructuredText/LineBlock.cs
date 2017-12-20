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
using System.Runtime.InteropServices;

namespace ReStructuredText
{
    public class LineBlock : IElement
    {
        public ElementType TypeCode => ElementType.LineBlock;
        public IList<ITextArea> TextAreas => Elements[0].TextAreas;
        public IParent Parent { get; set; }
        public IList<IElement> Elements { get; }

        public LineBlock(IList<Line> lines)
        {
            Elements = new List<IElement>();
            // TODO: improve indentation parsing.
            foreach (var line in lines)
            {
                if (line.TextAreas[0].IsIndented)
                {
                    line.TextAreas[0].Indentation = 0;
                    Elements.Add(new LineBlock(new[] {line}));
                    continue;
                }
                
                Elements.Add(line);
            }
        }
    }
}