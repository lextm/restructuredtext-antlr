using System.Collections.Generic;

namespace ReStructuredText
{
    public class LiteralBlock : IElement
    {
        public LiteralBlock(IList<Line> lines)
        {
            Lines = lines;
            var minimal = lines[0].Indentation;
            foreach (var line in Lines)
            {
                line.Text.RemoveEnd();
                if (line.Indentation < minimal)
                {
                    minimal = line.Indentation;
                }
            }

            foreach (var line in Lines)
            {
                if (line.Indentation > minimal)
                {
                    line.Text.Append(line.Indentation - minimal);
                    line.Indentation = minimal;
                }
            }
        }

        public ElementType TypeCode => ElementType.LiteralBlock;
        public IList<Line> Lines { get; }
        public IParent Parent { get; set; }
    }
}