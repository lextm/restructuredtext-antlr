using System.Collections.Generic;
using System.Linq;

namespace ReStructuredText
{
    public class Section : IElement, IParent
    {
        public int Level { get; }
        public IList<IElement> Content { get; }
        public string Title { get; set; }

        public Section(int level, string title, IList<IElement> content)
        {
            Title = title.Trim();
            Level = level;
            Content = new List<IElement>();
            foreach (var item in content)
            {
                item.Parent = this;
                if (item.Lines.Count == 0)
                {
                    continue;
                }

                Content.Add(item);
            }
        }

        public ElementType TypeCode => ElementType.Section;

        // TODO:
        public IList<Line> Lines => Content[0].Lines;

        public IParent Parent { get; set; }

        public void Add(IElement current, int level = 0)
        {
            if (!(current is Section section) || section.Level == Level + 1)
            {
                Content.Add(current);
                current.Parent = this;
                return;
            }

            if (section.Level <= Level)
            {
                Parent?.Add(current);
            }
            else
            {
                var child = Content.LastOrDefault() as Section;
                child?.Add(current);
            }
        }
    }
}
