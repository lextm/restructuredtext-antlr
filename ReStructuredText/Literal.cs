namespace Lextm.ReStructuredText
{
    public class Literal : ITextArea
    {
        private readonly TextArea _textArea;

        public Literal(TextArea textArea)
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
        public ElementType TypeCode => ElementType.Literal;

        public Scope Scope => _textArea.Scope;
    }
}