﻿namespace ReStructuredText
{
    public class Text
    {
        public Text(string text)
        {
            Content = text;
        }

        public string Content { get; private set; }

        public void RemoveList()
        {
            Content = Content.Substring(2);
        }
    }
}
