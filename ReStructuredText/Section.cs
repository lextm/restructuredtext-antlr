using System.Collections.Generic;
using System.Linq;

namespace ReStructuredText
{
    public class Section : IElement, IParent
    {
        public int Level { get; }
        public IList<IElement> Content { get; }
        public string Title { get; set; }

        public Section(int level, string title, params IElement[] content)
        {
            Title = title;
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

        internal static Section Parse(ref IElement current)
        {
            if (current.Lines.Count < 2)
            {
                return null;
            }

            if (current.Lines[1].IsSection)
            {
                var title = current.Lines[0].Text.Content.TrimEnd();
                current.Lines.RemoveAt(0);
                var line = current.Lines[0].Text.Content;
                var level = SectionTracker.Instance.Track(line[0]);
                current.Lines.RemoveAt(0);
                var result = new Section(level, title, current);
                current = result;
                return result;
            }

            // overline title.
            if (current.Lines.Count > 2 && current.Lines[0].IsSection && current.Lines[2].IsSection)
            {
                current.Lines.RemoveAt(0);
                var title = current.Lines[0].Text.Content.TrimEnd();
                current.Lines.RemoveAt(0);
                var line = current.Lines[0].Text.Content;
                var level = SectionTracker.Instance.Track(line[0]);
                current.Lines.RemoveAt(0);
                var result = new Section(level, title, current);
                current = result;
                return result;
            }

            return null;
        }

        class SectionTracker
        {
            public static readonly SectionTracker Instance = new SectionTracker();
            private readonly IList<char> _sections = new List<char>();

            public int Track(char item)
            {
                if (!_sections.Contains(item))
                {
                    _sections.Add(item);
                }

                return _sections.IndexOf(item) + 1;
            }
        }
    }
}
