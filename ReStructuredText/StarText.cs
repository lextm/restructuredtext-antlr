using System.Collections.Generic;
using System.Linq;

namespace ReStructuredText
{
    public class StarText : ITextArea
    {
        private readonly TextArea _textArea;

        public StarText(TextArea textArea)
        {
            _textArea = textArea;
        }

        public bool IsIndented => _textArea.IsIndented;
        public Content Content => _textArea.Content;
        public int Indentation
        {
            get => _textArea.Indentation;
            set => _textArea.Indentation = value;
        }

        public bool IsQuoted => _textArea.IsQuoted;
        public ElementType TypeCode => ElementType.StarText;

        public void Process(IList<ITextArea> list)
        {
            var content = _textArea.Content.Text;
            int level = 0;
            var start = 0;
            var length = 0;
            var maxLevel = 0;
            var count = 0;
            for (int i = 0; i < content.Length; i++)
            {
                int previous = i - 1;
                var previousChar = previous >= 0 ? content[previous] : char.MinValue;
                if (content[i] == '*' && previousChar != '\\')
                {
                    count++;
                }
            }

            var balanced = count % 2 == 0;
            for (int i = 0; i < content.Length; i++)
            {
                int previous = i - 1;
                var previousChar = previous >= 0 ? content[previous] : char.MinValue;
                if (content[i] == '*' && previousChar != '\\')
                {
                    if (length == 0)
                    {
                        // still in leading back ticks.
                        level++;
                        maxLevel = level;
                        start = i;
                        continue;
                    }

                    if (!balanced && i > 0)
                    {
                        length++;
                        balanced = true;
                        continue;
                    }

                    if (previousChar == ' ' && level > 0)
                    {
                        length++;
                        continue;
                    }
                    
                    level--;
                    if (level > 0)
                    {
                        // in ending back ticks.
                        continue;
                    }

                    var part = content.Substring(start + 1, length);
                    if (maxLevel == 2)
                    {
                        list.Add(new Strong(TextArea.Parse(part)));
                    }
                    else if (maxLevel == 1)
                    {
                        list.Add(new Emphasis(TextArea.Parse(part)));
                    }
                    else if (maxLevel == 0)
                    {
                        foreach (var item in TextArea.Parse(part))
                        {
                            list.Add(item);
                        }
                    }
                    else
                    {
                        list.Add(new Strong(TextArea.Parse($"*{part}*")));
                    }

                    maxLevel = 0;
                    if (level < 0)
                    {
                        // restart
                        maxLevel = level = 1;
                    }

                    length = 0;
                    start = i;
                    continue;
                }

                length++;
            }
        }

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