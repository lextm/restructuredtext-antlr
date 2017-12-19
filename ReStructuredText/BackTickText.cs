using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReStructuredText
{
    public class BackTickText : ITextArea
    {
        public string Title { get; set; }
        private readonly TextArea _textArea;

        public BackTickText(string title, TextArea textArea)
        {
            Title = title;
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
        public ElementType TypeCode => ElementType.BackTickText;

        public void Process(IList<ITextArea> list)
        {
            int level = 0;
            var content = _textArea.Content.Text;
            var start = 0;
            var length = 0;
            var maxLevel = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == '`')
                {
                    if (length == 0)
                    {
                        // still in leading back ticks.
                        level++;
                        maxLevel = level;
                        start = i;
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
                        list.Add(new Literal(new TextArea(part)));
                    }
                    else if (maxLevel == 1)
                    {
                        var title = Title;
                        if (title == null)
                        {
                            var last = list.LastOrDefault();
                            if (last is TextArea text)
                            {
                                title = text.Content.RemoveTitle();
                            }
                        }
                        else
                        {
                            Title = null;
                        }

                        list.Add(new InterpretedText(title, new TextArea(part)));
                    }
                    else if (maxLevel == 0)
                    {
                        list.Add(new TextArea(part));
                    }
                    else
                    {
                        list.Add(new Literal(new TextArea($"`{part}`")));
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
    }
}