using System.Collections.Generic;

namespace ReStructuredText
{
    public class Comment : IElement
    {
        public IList<Line> Lines { get; }

        public Comment(IList<Line> lines)
        {
            Lines = new List<Line>();
            bool skip = true;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line.Text.Content))
                {
                    if (skip)
                    {
                        continue;
                    }
                }

                skip = false;
                Lines.Add(line);
            }
        }

        public ElementType TypeCode => ElementType.Comment;

        public IParent Parent { get; set; }
    }
}
