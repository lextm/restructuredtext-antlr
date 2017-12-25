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
    public class LiteralBlock : IElement
    {
        public LiteralBlock(IList<ITextArea> lines)
        {
            TextAreas = lines;
            var minimal = lines[0].Indentation;
            foreach (var line in TextAreas)
            {
                line.Content.RemoveEnd();
                if (line.Indentation < minimal)
                {
                    minimal = line.Indentation;
                }
            }

            foreach (var line in TextAreas)
            {
                if (line.Indentation > minimal)
                {
                    line.Content.Append(line.Indentation - minimal);
                    line.Indentation = minimal;
                }
            }
        }

        public ElementType TypeCode => ElementType.LiteralBlock;
        public IList<ITextArea> TextAreas { get; }
        public IParent Parent { get; set; }

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