using System.Collections.Generic;

namespace ReStructuredText
{
    public class Document
    {
        public Document(IList<IElement> elements)
        {
            Elements = elements;
        }

        public IList<IElement> Elements { get; }
    }
}
