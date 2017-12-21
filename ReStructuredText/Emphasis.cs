using System.Collections.Generic;
using System.Linq;

namespace ReStructuredText
{
    public class Emphasis : ITextArea
    {
        public IList<ITextArea> TextAreas { get; }

        public Emphasis(IList<ITextArea> textAreas)
        {
            TextAreas = textAreas;
        }

        public bool IsIndented => TextAreas[0].IsIndented;
        public Content Content => TextAreas[0].Content;
        public int Indentation
        {
            get => TextAreas[0].Indentation;
            set => TextAreas[0].Indentation = value;
        }

        public bool IsQuoted => TextAreas[0].IsQuoted;
        public ElementType TypeCode => ElementType.Emphasis;

        public static void Deemphasize(IList<ITextArea> textAreas)
        {
            for (var i = 0; i < textAreas.Count; i++)
            {
                var area = textAreas[i];
                if (area is Emphasis emphasis)
                {
                    if (i == 0 || i == textAreas.Count - 1)
                    {
                        continue;
                    }

                    var last = textAreas[i - 1];
                    var next = textAreas[i + 1];
                    var lastChar = last.Content.Text.LastOrDefault();
                    var nextChar = next.Content.Text.FirstOrDefault();
                    if ((lastChar == ')' && nextChar == '(')
                        || (lastChar == ']' && nextChar == '[')
                        || (lastChar == '>' && nextChar == '>') // TODO: verify this.
                        || (lastChar == '}' && nextChar == '{'))
                    {
                        last.Content.Append($"*{area.Content.Text}*");
                        last.Content.Append(next.Content.Text);
                        textAreas.Remove(area);
                        textAreas.Remove(next);
                        i--;
                    }
                }
            }
        }
    }
}