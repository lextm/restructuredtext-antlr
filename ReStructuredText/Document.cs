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
        
        public void Add(IElement element, int level = 0)
        {
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
                if (current.TypeCode == ElementType.Comment)
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

                if (!(Elements.LastOrDefault() is BlockQuote block))
                {
                    Section newSection = null;
                    if (current.Lines[0].IsIndented)
                    {
                        var level = current.Lines[0].Indentation / indentation;
                        while (level > 0)
                        {
                            current = new BlockQuote(level, current);
                            level--;
                        }
                    }
                    else
                    {
                        newSection = Section.Parse(ref current);
                    }

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
