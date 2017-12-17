using System.Collections.Generic;
using System.Linq;

namespace ReStructuredText
{
    public class ListItem
    {
        public IList<Paragraph> Elements { get; }

        public ListItem(char start, Paragraph element)
        {
            Start = start;
            Elements = new List<Paragraph>{element};
            element.Lines[0].Text.RemoveList();
        }
        
        public char Start { get; }

        public static IList<ListItem> Parse(char bullet, Paragraph paragraph)
        {
            var start = paragraph.Lines[0].Text.Content[0];
            var pivot = 0;
            for (var index = 1; index < paragraph.Lines.Count; index++)
            {
                var line = paragraph.Lines[index];
                if (line.BulletChar != char.MinValue)
                {
                    pivot = index;
                    break;
                }
            }

            if (pivot > 0)
            {
                var newLines = paragraph.Lines.Take(pivot);
                var result = new List<ListItem>
                    {new ListItem(start, new Paragraph(newLines.ToList()))};
                result.AddRange(Parse(bullet, new Paragraph(paragraph.Lines.Skip(pivot).ToList())));
                return result;
            }
            
            return new[] {new ListItem(start, paragraph)};
        }
    }
}