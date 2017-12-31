using System.Collections.Generic;

namespace Lextm.ReStructuredText
{
    public class Definition
    {
        public Definition()
        {
            Elements = new List<IElement>();
        }

        public IList<IElement> Elements { get; }

        public IParent Add(IElement element)
        {
            Elements.Add(element);
            return element;
        }
        
        public int Indentation { get; set; }
    }
}