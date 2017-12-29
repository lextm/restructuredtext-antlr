using System.Collections.Generic;
using System.Linq;

namespace Lextm.ReStructuredText
{
    public class Term
    {
        public Term(IEnumerable<ITextArea> take)
        {
            take.Last().Content.RemoveEnd();
            TextAreas = take.ToList();
        }

        public IList<ITextArea> TextAreas { get; }
    }
}