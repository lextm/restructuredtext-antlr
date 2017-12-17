using System.Collections.Generic;
using System.Linq;

namespace ReStructuredText
{
    public class ListItem : IElement
    {
        public IList<Paragraph> Elements { get; }

        public ListItem(char start, IList<Paragraph> element)
        {
            Start = start;
            Elements = element;
        }
        
        public char Start { get; }

        public ElementType TypeCode => ElementType.ListItem;
        public IList<Line> Lines => Elements[0].Lines;
        public IParent Parent { get; set; }
    }
}