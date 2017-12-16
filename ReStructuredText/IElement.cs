using System.Collections.Generic;

namespace ReStructuredText
{
    public interface IElement
    {
        ElementType TypeCode { get; }

        IList<Line> Lines { get; }

        IParent Parent { get; set; }
    }
}
