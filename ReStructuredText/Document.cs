using System.Collections.Generic;

namespace ReStructuredText
{
    public class Document
    {
        public Document(IList<Paragraph> paragraphs)
        {
            this.Paragraphs = paragraphs;
        }

        public IList<Paragraph> Paragraphs { get; }
    }
}
