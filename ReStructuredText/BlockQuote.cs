using System;
using System.Collections.Generic;

namespace ReStructuredText
{
    public class BlockQuote : IElement
    {
        public int Level { get; }
        public IList<IElement> Content { get; }

        public BlockQuote(int level, params IElement[] content)
        {
            Level = level;
            Content = new List<IElement>(content);
            foreach (var item in Content)
            {
                item.Block = this;
            }
        }

        public ElementType TypeCode => ElementType.BlockQuote;

        // TODO:
        public IList<Line> Lines => Content[0].Lines;

        public BlockQuote Block { get; set; }

        internal void Eat(IElement current, int level)
        {
            if (level == Level)
            {
                Content.Add(current);
                current.Block = this;
                return;
            }

            if (level < Level)
            {
                Block?.Eat(current, level);
                return;
            }

            while (level > Level)
            {
                current = new BlockQuote(level, current);
                level--;
            }

            Content.Add(current);
            current.Block = this;
        }
    }
}
