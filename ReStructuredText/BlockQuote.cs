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
    public class BlockQuote : IElement, IParent
    {
        public int Level { get; }
        public IList<IElement> Elements { get; }

        public BlockQuote(int level, params IElement[] content)
        {
            Level = level;
            Elements = new List<IElement>(content);
            foreach (var item in Elements)
            {
                item.Parent = this;
            }
        }

        public ElementType TypeCode => ElementType.BlockQuote;

        // TODO:
        public IList<ITextArea> TextAreas => Elements[0].TextAreas;

        public IParent Parent { get; set; }

        public void Add(IElement current, int level)
        {
            if (level == Level)
            {
                Elements.Add(current);
                current.Parent = this;
                return;
            }

            if (level < Level)
            {
                var block = Parent as BlockQuote;
                block?.Add(current, level);
                return;
            }

            while (level > Level)
            {
                current = new BlockQuote(level, current);
                level--;
            }

            Elements.Add(current);
            current.Parent = this;
        }
    }
}
