using System.Collections.Generic;

namespace ReStructuredText
{
    public class BlockQuote : IElement, IParent
    {
        public int Level { get; }
        public IList<IElement> Content { get; }

        public BlockQuote(int level, params IElement[] content)
        {
            Level = level;
            Content = new List<IElement>(content);
            foreach (var item in Content)
            {
                item.Parent = this;
            }
        }

        public ElementType TypeCode => ElementType.BlockQuote;

        // TODO:
        public IList<Line> Lines => Content[0].Lines;

        public IParent Parent { get; set; }

        public void Add(IElement current, int level)
        {
            if (level == Level)
            {
                Content.Add(current);
                current.Parent = this;
                return;
            }

            if (level < Level)
            {
                var block = Parent as BlockQuote;
                block?.Add(current, level);
                return;
            }

            while (level > Level)
            {
                current = new BlockQuote(level, current);
                level--;
            }

            Content.Add(current);
            current.Parent = this;
        }
    }
}
