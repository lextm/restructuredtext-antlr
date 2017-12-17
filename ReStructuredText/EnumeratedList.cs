using System.Collections.Generic;
using System.Linq;

namespace ReStructuredText
{
    public class EnumeratedList : IElement
    {
        public IList<ListItem> Items { get; }
        
        public EnumeratedList(ListItem item)
        {
            Items = new List<ListItem>{item};
        }

        public ElementType TypeCode => ElementType.EnumeratedList;
        public IList<Line> Lines => Items[0].Lines;
        public IParent Parent { get; set; }

        public void Add(IElement current)
        {
            Items.LastOrDefault()?.Elements.Add((Paragraph)current);
        }
    }
}