using System;
using System.Collections.Generic;
using System.Linq;

namespace Lextm.ReStructuredText
{
    public class DefinitionListItem : IElement
    {
        private DefinitionListItem(IList<ITextArea> take, IEnumerable<ITextArea> skip, int unit, IParent parent)
        {
            var term = new List<ITextArea>();
            var rest = new List<ITextArea>();
            foreach (var item in take)
            {
                if (item.TypeCode == ElementType.Text)
                {
                    if (string.IsNullOrWhiteSpace(item.Content.Text))
                    {
                        continue;
                    }

                    var content = item.Content.Text;
                    int index;
                    while ((index = content.IndexOf(" : ", StringComparison.Ordinal)) > -1)
                    {
                        var text = new TextArea(content.Substring(0, index), item.Scope);
                        if (Term == null)
                        {
                            term.Add(text);
                            Term = new Term(term);
                        }
                        else
                        {
                            var list = new List<ITextArea>();
                            if (rest.Count > 0)
                            {
                                list.AddRange(rest);
                                rest.Clear();
                            }

                            list.Add(text);
                            Classifiers.Add(new Classifier(list));
                        }

                        content = content.Substring(index + " : ".Length);
                    }

                    if (string.IsNullOrWhiteSpace(content))
                    {
                        continue;
                    }

                    var last = new TextArea(content, item.Scope);
                    if (Term == null)
                    {
                        term.Add(last);
                    }
                    else
                    {
                        rest.Add(last);
                    }

                    continue;
                }

                if (Term == null)
                {
                    term.Add(item);
                }
                else
                {
                    rest.Add(item);
                }
            }

            if (Term == null)
            {
                Term = new Term(term);
            }

            Term.TextAreas.First().Indentation = take.First().Indentation;
            if (rest.Count > 0)
            {
                Classifiers.Add(new Classifier(rest));
            }

            var paragraph = new Paragraph(skip) { Unit = unit };
            Definition = new Definition {Indentation = paragraph.Indentation};
            Definition.Add(paragraph.Parse(paragraph.Indentation, paragraph.Unit, this).Item2);
        }

        public IList<Classifier> Classifiers { get; } = new List<Classifier>();

        public Definition Definition { get; }

        public Term Term { get; }

        public ElementType TypeCode => ElementType.DefinitionListItem;
        public IList<ITextArea> TextAreas { get; }
        public IParent Parent { get; set; }
        
        public IElement Find(int line, int column)
        {
            return null;
        }

        public static IList<DefinitionListItem> Parse(Paragraph paragraph, IParent last)
        {
            if (paragraph.TextAreas.Count < 2)
            {
                return Array.Empty<DefinitionListItem>();
            }

            var result = new List<DefinitionListItem>();
            IEnumerable<ITextArea> term = null;
            IList<ITextArea> rest = paragraph.TextAreas.ToList();
            var firstIndentation = paragraph.TextAreas[0].Indentation;
            while (rest.Count > 0)
            {
                for (var index = 0; index < rest.Count; index++)
                {
                    var item = rest[index];
                    if (item.Content.Text.Last() != '\n')
                    {
                        continue;
                    }
                    
                    var next = index + 1;
                    if (index + 1 == rest.Count)
                    {
                        if (term == null)
                        {
                            return Array.Empty<DefinitionListItem>();
                        }

                        result.Add(new DefinitionListItem(term.ToList(), rest, paragraph.Unit, last));
                        return result;
                    }

                    var indentation = rest[next].Indentation;
                    if (term == null)
                    {
                        if (indentation > firstIndentation)
                        {
                            term = rest.Take(next);
                            rest = rest.Skip(next).ToList();
                            break;
                        }
                    }
                    else if (indentation == firstIndentation)
                    {
                        // another term starts.
                        var body = rest.Take(next);
                        rest = rest.Skip(next).ToList();
                        result.Add(new DefinitionListItem(term.ToList(), body, paragraph.Unit, last));
                        term = null;
                        break;
                    }
                }
            }

            return Array.Empty<DefinitionListItem>();
        }

        public IParent Add(IElement element, int level = 0)
        {
            if (element.Indentation == Definition.Indentation)
            {
                element.Parent = this;
                return Definition.Add(element);
            }

            return Parent.Add(element);
        }

        public int Indentation => Term.TextAreas[0].Indentation;
    }
}