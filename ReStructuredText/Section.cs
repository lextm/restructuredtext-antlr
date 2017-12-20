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
    public class Section : IElement, IParent
    {
        public int Level { get; }
        public IList<IElement> Elements { get; }
        public IList<ITextArea> Title { get; set; }

        public Section(int level, IList<ITextArea> title, IList<IElement> content)
        {
            Title = new List<ITextArea>();
            foreach (var area in title)
            {
                if (area.TypeCode == ElementType.BackTickText)
                {
                    ((BackTickText)area).Process(TextAreas);
                }
                else if (area.TypeCode == ElementType.StarText)
                {
                    ((StarText)area).Process(Title);
                }
                else
                {
                    Title.Add(area);
                }
            }
            
            Title.First().Content.RemoveStart();
            Title.Last().Content.RemoveEnd();
            if (title.Last().Content.Text.Length == 0)
            {
                Title.Remove(title.Last());
            }
            
            Level = level;
            Elements = new List<IElement>();
            foreach (var item in content)
            {
                item.Parent = this;
                if (item.TextAreas.Count == 0)
                {
                    continue;
                }

                Elements.Add(item);
            }
        }

        public ElementType TypeCode => ElementType.Section;

        public IList<ITextArea> TextAreas => Title;

        public IParent Parent { get; set; }

        public void Add(IElement current, int level = 0)
        {
            if (!(current is Section section) || section.Level == Level + 1)
            {
                Elements.Add(current);
                current.Parent = this;
                return;
            }

            if (section.Level <= Level)
            {
                Parent?.Add(current);
            }
            else
            {
                var child = Elements.LastOrDefault() as Section;
                child?.Add(current);
            }
        }
    }
}
