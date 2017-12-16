using System.Collections.Generic;
using System.Linq;

namespace ReStructuredText
{
    public class BulletList : IElement
    {
        public IList<ListItem> Items { get; }
        
        public BulletList(ListItem listItem)
        {
            Items = new List<ListItem> {listItem};
            Start = listItem.Start;
        }

        public ElementType TypeCode => ElementType.BulletList;
        public IList<Line> Lines { get; }
        public IParent Parent { get; set; }
        public char Start { get; }

        public void Add(IElement current)
        {
            Items.LastOrDefault()?.Elements.Add((Paragraph)current);
        }
    }
}