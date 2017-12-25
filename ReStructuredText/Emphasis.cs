using System.Collections.Generic;
using System.Linq;

namespace Lextm.ReStructuredText
{
    public class Emphasis : ITextArea
    {
        public IList<ITextArea> TextAreas { get; }
        public Scope Scope { get; }

        public Emphasis(IList<ITextArea> textAreas)
        {
            TextAreas = textAreas;
            Scope = new Scope
            {
                LineStart = TextAreas.First().Scope.LineStart,
                LineEnd = TextAreas.Last().Scope.LineEnd
            };
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
    }
}