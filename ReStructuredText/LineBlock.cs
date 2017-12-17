using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ReStructuredText
{
    public class LineBlock : IElement
    {
        public ElementType TypeCode => ElementType.LineBlock;
        public IList<Line> Lines { get; }
        public IParent Parent { get; set; }
        public IList<IElement> Content { get; }

        public LineBlock(IList<Line> lines)
        {
            Content = new List<IElement>();
            Lines = new List<Line>();
            // TODO: improve indentation parsing.
            foreach (var line in lines)
            {
                line.Text.RemoveEnd();
                if (line.IsIndented)
                {
                    line.Indentation = 0;
                    Content.Add(new LineBlock(new[] {line}));
                    continue;
                }
                
                Lines.Add(line);
            }
        }
    }
}