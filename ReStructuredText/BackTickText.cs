using System.Collections.Generic;
using System.Linq;

namespace Lextm.ReStructuredText
{
    public class BackTickText : ITextArea
    {
        public string Title { get; set; }
        public Scope Scope { get; }

        private string _content;

        public BackTickText(string title, string content, Scope scope)
        {
            Title = title;
            Scope = scope;
            _content = content;
        }

        public bool IsIndented { get; }
        public Content Content { get; }
        public int Indentation
        {
            get; set;
        }

        public bool IsQuoted { get; }
        public ElementType TypeCode => ElementType.BackTickText;

        public void Process(IList<ITextArea> list)
        {
            var content = _content;
            if (content.Length == 2)
            {
                list.Add(new InterpretedText(Title, new TextArea(string.Empty, Scope)));
                return;
            }

            int level = 0;
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
                        list.Add(new Literal(new TextArea(part, Scope)));
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
                            // pick up title.
                            Title = null;
                        }

                        list.Add(new InterpretedText(title, new TextArea(part, Scope)));
                    }
                    else if (maxLevel == 0)
                    {
                        list.Add(new TextArea(part, Scope));
                    }
                    else
                    {
                        list.Add(new Literal(new TextArea($"`{part}`", Scope)));
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