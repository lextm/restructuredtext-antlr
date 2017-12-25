using System.Collections.Generic;
using System.Linq;

namespace Lextm.ReStructuredText
{
    public class StarText : ITextArea
    {
        private readonly string _content;

        public StarText(string content, Scope scope)
        {
            _content = content;
            Scope = scope;
        }

        public bool IsIndented { get; }
        public Content Content { get; }
        public int Indentation { get; set; }

        public bool IsQuoted { get; }
        public ElementType TypeCode => ElementType.StarText;

        public Scope Scope { get; }

        public void Process(IList<ITextArea> owner)
        {
            var content = _content;
            var clean = content.TrimEnd();
            if (clean.Count(_ => _ == '*') == clean.Length)
            {
                if (clean.Length > 4)
                {
                    owner.Add(Strong.ParseStars(clean, Scope));
                    owner.Add(new TextArea("\n", Scope));
                }
                else
                {
                    owner.Add(new TextArea(clean + '\n', Scope));
                }

                return;
            }
            
            int level = 0;
            var length = 0;
            var maxLevel = 0;
            var list = new List<ITextArea>();
            var balanced = true;
            var lastLineStart = 0;
            var count = 0;
            ITextArea lastText = null;
            int currentLine = Scope.LineStart;
            for (int j = 0; j < content.Length; j++)
            {
                if (content[j] == '*')
                {
                    var fragment = content.Substring(j - length - (maxLevel == 0 ? 0 : maxLevel - 1), length);
                    var escaped = IsEscaped(j, content, level > 0, ref balanced);
                    if (escaped)
                    {
                        length++;
                    }
                    else
                    {
                        count++;
                        if (length > 0 && maxLevel == 0)
                        {
                            foreach (var item in TextArea.Parse(fragment, new Scope { LineStart = currentLine, LineEnd = currentLine }))
                            {
                                list.Add(item);
                            }

                            length = 0;
                        }

                        if (length == 0)
                        {
                            level++;
                            maxLevel = level;
                        }
                        else
                        {
                            level--;
                        }

                        if (level == 0)
                        {
                            if (maxLevel == 2)
                            {
                                list.Add(new Strong(TextArea.Parse(fragment, new Scope { LineStart = currentLine, LineEnd = currentLine })));
                            }
                            else if (maxLevel == 1)
                            {
                                list.Add(new Emphasis(TextArea.Parse(fragment, new Scope { LineStart = currentLine, LineEnd = currentLine })));
                            }
                            else
                            {
                                list.Add(new Strong(TextArea.Parse($"*{fragment}*", new Scope { LineStart = currentLine, LineEnd = currentLine })));
                            }

                            length = 0;
                            maxLevel = 0;
                        }
                    }
                }
                else
                {
                    length++;
                }

                if (content[j] == '\n')
                {
                    currentLine++;
                    if (j != content.Length - 1)
                    {
                        lastLineStart = j + 1;
                        foreach (var item in list)
                        {
                            owner.Add(item);
                        }

                        list.Clear();
                    }
                }
            }

            balanced = count % 2 == 0;
            if (balanced)
            {
                foreach (var item in list)
                {
                    owner.Add(item);
                }

                list.Clear();
                
                if (length > 0)
                {
                    var fragment = content.Substring(content.Length - length, length);
                    lastText = TextArea.Parse(fragment, new Scope { LineStart = currentLine, LineEnd = currentLine })[0];
                }
            }
            else
            {
                level = 0;
                length = 0;
                maxLevel = 0;
                // rework last line.
                for (int j = lastLineStart == 0 ? 0 : lastLineStart + 1; j < content.Length; j++)
                {
                    if (content[j] == '*')
                    {
                        var escaped = IsEscaped(j, content, level > 0, ref balanced);
                        if (escaped)
                        {
                            length++;
                        }
                        else
                        {
                            count++;
                            var fragment = content.Substring(j - length - (maxLevel == 0 ? 0 : maxLevel - 1), length);
                            if (length > 0 && maxLevel == 0)
                            {
                                foreach (var item in TextArea.Parse(fragment, new Scope { LineStart = currentLine, LineEnd = currentLine }))
                                {
                                    owner.Add(item);
                                }

                                length = 0;
                            }

                            if (length == 0)
                            {
                                level++;
                                maxLevel = level;
                            }
                            else
                            {
                                level--;
                            }

                            if (level == 0)
                            {
                                if (maxLevel == 2)
                                {
                                    owner.Add(new Strong(TextArea.Parse(fragment, new Scope { LineStart = currentLine, LineEnd = currentLine })));
                                }
                                else if (maxLevel == 1)
                                {
                                    owner.Add(new Emphasis(TextArea.Parse(fragment, new Scope { LineStart = currentLine, LineEnd = currentLine })));
                                }
                                else
                                {
                                    owner.Add(new Strong(TextArea.Parse($"*{fragment}*", new Scope { LineStart = currentLine, LineEnd = currentLine })));
                                }

                                length = 0;
                                maxLevel = 0;
                            }
                        }
                    }
                    else
                    {
                        length++;
                    }
                }
                
                if (length > 0)
                {
                    var fragment = content.Substring(content.Length - length, length);
                    lastText = TextArea.Parse(fragment, new Scope { LineStart = currentLine, LineEnd = currentLine })[0];
                }
            }
            
            if (lastText != null)
            {
                owner.Add(lastText);
            }
        }

        private static bool IsEscaped(int index, string content, bool inStar, ref bool balanced)
        {
            var previousChar2 = index - 2 >= 0 ? content[index - 2] : char.MinValue;
            var previousChar1 = index - 1 >= 0 ? content[index - 1] : char.MinValue;
            var nextChar1 = index + 1 < content.Length ? content[index + 1] : char.MinValue;
            var nextChar2 = index + 2 < content.Length ? content[index + 2] : char.MinValue;

            if (previousChar1 == '\\')
            {
                return true;
            }

            if (previousChar2 == '\\' && previousChar1 == '*' && (nextChar1 != '\n' && nextChar2 != '*'))
            {
                return true;
            }

            if (char.IsDigit(previousChar1) || previousChar1 == 'x')
            {
                return true;
            }
            
            if (!inStar && nextChar1 == ' ')
            {
                return true;
            }

            if (!inStar && previousChar1 == ' ' && nextChar1 == '*' && (nextChar2 == '\n' || nextChar2 == '\r'))
            {
                return true;
            }

            if (!inStar && previousChar2 == ' ' && (nextChar1 == '\n' || nextChar1 == '\r'))
            {
                return true;
            }

            if (inStar && previousChar1 == ' ')
            {
                return true;
            }

            if (previousChar1 == '[' && nextChar1 == ']') 
            {   
                // [*]
                return true;
            }

            if (previousChar1 == '(' && nextChar1 == ')')
            {   
                // (*)
                return true;
            }

            if (previousChar1 == '(' && nextChar1 == '*')
            {
                // (**
                return true;
            }

            if (previousChar2 == '(' && previousChar1 == '*' && (nextChar1 == ')' || nextChar1 == ' '))
            {
                // (**) or (** 
                return true;
            }

            if (previousChar1 == '*' && char.IsDigit(nextChar1))
            {
                return true;
            }

            if (previousChar1 == '\'' && nextChar1 == '\'')
            {
                // '*'
                return true;
            }

            if (previousChar1 == '"' && previousChar2 == '\'' && nextChar1 == '"' && nextChar2 == '\'')
            {
                // '"*"'
                return true;
            }
            
            if (inStar && !balanced)
            {
                balanced = true;
                return true;
            }

            return false;
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