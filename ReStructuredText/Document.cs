using System.Collections.Generic;
using System.Linq;

namespace ReStructuredText
{
    public class Document : IParent
    {
        public Document()
        {
            Elements = new List<IElement>();
        }

        public IList<IElement> Elements { get; }

        public void Add(IList<ListItem> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }
        
        private void Add(ListItem item)
        {
            if (!(Elements.LastOrDefault() is BulletList list) || list.Start != item.Start)
            {
                list = new BulletList(item);
                Elements.Add(list);
            }
            else
            {
                list.Items.Add(item);
            }
        }
        
        public void Add(IElement element, int level = 0)
        {
            if (element is ListItem listItem)
            {
                Add(listItem);
                return;
            }
            
            Elements.Add(element);
            element.Parent = this;
        }

        public void Eat(List<IElement> raw)
        {
            var indentation = IndentationTracker.Instance.Minimum;
            Section section = null;
            // IMPORTANT: block quote processing
            for (int i = 0; i < raw.Count; i++)
            {
                var current = raw[i];
                if (current.TypeCode == ElementType.Comment || current.TypeCode == ElementType.ListItem)
                {
                    if (section == null)
                    {
                        Add(current);
                    }
                    else
                    {
                        section.Add(current);
                    }

                    continue;
                }

                if (current.TypeCode == ElementType.Section)
                {
                    var newSection = current as Section;  
                    if (section == null)
                    {
                        Add(current);
                    }
                    else
                    {
                        section.Add(current);
                    }
                    
                    section = newSection ?? section;
                    continue;
                }

                if (!(Elements.LastOrDefault() is BlockQuote block))
                {
                    if (current.Lines[0].IsIndented || current.Lines[0].IsQuoted)
                    {
                        var last = Elements.LastOrDefault()?.Lines?.LastOrDefault()?.Text.Content.TrimEnd();
                        if (last != null && last.EndsWith("::"))
                        {
                            current = new LiteralBlock(current.Lines);
                            Elements.Last().Lines.Last().Text.RemoveLiteral();
                        }
                        else
                        {
                            if (Elements.LastOrDefault() is BulletList list)
                            {
                                if (current.Lines[0].Indentation == 2)
                                {
                                    list.Add(current);
                                    continue;
                                }
                            }

                            var level = current.Lines[0].Indentation / indentation;
                            while (level > 0)
                            {
                                current = new BlockQuote(level, current);
                                level--;
                            }
                        }
                    }

                    if (section == null)
                    {
                        Add(current);
                    }
                    else
                    {
                        section.Add(current);
                    }

                    continue;
                }

                if (current.Lines[0].IsIndented)
                {
                    var level = current.Lines[0].Indentation / indentation;
                    block.Add(current, level);
                }
                else
                {
                    if (section == null)
                    {
                        Add(current);
                    }
                    else
                    {
                        section.Add(current);
                    }
                }
            }
        }

        internal class IndentationTracker
        {
            public static readonly IndentationTracker Instance = new IndentationTracker();

            public void Track(int indentation)
            {
                if (indentation == 0)
                {
                    return;
                }

                if (Minimum > 0 && Minimum < indentation)
                {
                    return;
                }

                Minimum = indentation;
            }

            public int Minimum
            {
                get; private set;
            }
        }
    }
}
