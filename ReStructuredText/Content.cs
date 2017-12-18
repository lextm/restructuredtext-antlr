// Copyright (C) 2017 Lex Li
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ReStructuredText
{
    public class Content
    {
        public Content(string text)
        {
            Text = text;
            Unescape();
        }

        public string Text { get; private set; }

        public void RemoveEnd()
        {
            Text = Text.TrimEnd();
        }

        public void RemoveLiteral()
        {
            RemoveEnd();
            Text = Text.TrimEnd(':');
        }

        public void Append(int indentation)
        {
            var builder = new StringBuilder(Text.Length + indentation);
            for (int i = 0; i < indentation; i++)
            {
                builder.Append(' ');
            }

            Text = builder.Append(Text).ToString();
        }

        public void Append(string text)
        {
            Text = Text + text;
        }

        public void Unescape()
        {
            Regex regex = new Regex (@"\\U([0-9A-F]{4})", RegexOptions.IgnoreCase);
            Text = regex.Replace (Text, match => ((char)int.Parse (match.Groups[1].Value,
                NumberStyles.HexNumber)).ToString ());
        }
    }
}
