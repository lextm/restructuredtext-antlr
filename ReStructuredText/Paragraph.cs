using System.Collections.Generic;

namespace ReStructuredText
{
    public class Paragraph : IElement
    {
        public IList<Line> Lines { get; }

        public Paragraph(IList<Line> lines)
        {
            Lines = lines;
        }

        public bool IsBlockQuote => Lines.Count > 0 && Lines[0].IsIndented;

        public ElementType TypeCode => ElementType.Paragraph;

        public BlockQuote Block { get; set; }

        public override string ToString()
        {
            return Lines[0].Text.Content;
        }
    }
}
