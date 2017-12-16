using System.Collections.Generic;

namespace ReStructuredText
{
    public class Comment : IElement
    {
        public IList<Line> Lines { get; }

        public Comment(IList<Line> lines)
        {
            Lines = lines;
        }

        public ElementType TypeCode => ElementType.Comment;

        public BlockQuote Block { get; set; }
    }
}
