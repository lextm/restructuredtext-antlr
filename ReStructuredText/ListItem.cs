using System.Collections.Generic;

namespace ReStructuredText
{
    public class ListItem : IElement
    {
        public IList<Paragraph> Elements { get; }

        public ListItem(string start, string enumerator, IList<Paragraph> element)
        {
            Start = start?[0] ?? char.MinValue;
            Enumerator = enumerator;
            if (enumerator != null)
            {
                Index = int.Parse(Enumerator.TrimEnd('.', ' '));
            }
            
            Elements = element;
        }
        
        public char Start { get; }
        public string Enumerator { get; }
        public int Index { get; }

        public ElementType TypeCode => ElementType.ListItem;
        public IList<Line> Lines => Elements[0].Lines;
        public IParent Parent { get; set; }
        public bool HasEnding { get; set; }
    }
}