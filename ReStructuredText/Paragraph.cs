using System.Collections.Generic;

namespace ReStructuredText
{
    public class Paragraph
    {
        public IList<Line> Lines { get; }

        public Paragraph(IList<Line> lines)
        {
            Lines = lines;
        }
        
        public bool IsComment { get; internal set; }

        public bool IsBlockQuote => Lines.Count > 0 && Lines[0].IsIndented;
    }
}
