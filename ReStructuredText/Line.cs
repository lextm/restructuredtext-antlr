using System.Collections.Generic;
using System.Linq;

namespace Lextm.ReStructuredText
{
    public class Line : IElement
    {
        public Line(IList<ITextArea> areas)
        {
            TextAreas = new List<ITextArea>();
            areas.Last().Content.RemoveEnd();
            foreach (var area in areas)
            {
                if (area.TypeCode == ElementType.BackTickText)
                {
                    ((BackTickText)area).Process(TextAreas);
                }
                else if (area.TypeCode == ElementType.StarText)
                {
                    ((StarText)area).Process(TextAreas);
                }
                else
                {
                    TextAreas.Add(area);
                }
            }
        }
        
        public ElementType TypeCode => ElementType.Line;
        public IList<ITextArea> TextAreas { get; }
        public IParent Parent { get; set; }

        public IElement Find(int line, int column)
        {
            var first = TextAreas.First();
            var last = TextAreas.Last();
            if (line < first.Scope.LineStart || line > last.Scope.LineEnd)
            {
                return null;
            }

            return this;
        }
    }
}