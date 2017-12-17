using System;
using System.Text;

namespace ReStructuredText
{
    public class Text
    {
        public Text(string text)
        {
            Content = text;
        }

        public string Content { get; private set; }

        public void RemoveEnd()
        {
            Content = Content.TrimEnd();
        }

        public void RemoveLiteral()
        {
            RemoveEnd();
            Content = Content.TrimEnd(':');
        }

        public void Append(int indentation)
        {
            var builder = new StringBuilder(Content.Length + indentation);
            for (int i = 0; i < indentation; i++)
            {
                builder.Append(' ');
            }

            Content = builder.Append(Content).ToString();
        }
    }
}
