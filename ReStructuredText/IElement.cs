using System.Collections.Generic;

namespace ReStructuredText
{
    public interface IElement
    {
        ElementType TypeCode { get; }

        IList<Line> Lines { get; }

        BlockQuote Block { get; set; }
    }
}
