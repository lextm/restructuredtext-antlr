using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lextm.ReStructuredText
{
    public class Strong : ITextArea
    {
        public IList<ITextArea> TextAreas { get; }
        public Scope Scope { get; }

        public Strong(IList<ITextArea> textTextAreas)
        {
            TextAreas = textTextAreas;
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
        public ElementType TypeCode => ElementType.Strong;

        internal static ITextArea ParseStars(string stars, Scope scope)
        {
            var length = stars.Length;
            var builder = new StringBuilder(length - 4);
            for (int i = 0; i < length - 4; i++)
            {
                builder.Append("*");
            }

            return new Strong(new ITextArea[] { new TextArea(builder.ToString(), scope) });
        }
    }
}