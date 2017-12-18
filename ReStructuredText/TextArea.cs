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

namespace ReStructuredText
{
    public class TextArea : ITextArea
    {
        public Content Content { get; }

        public bool IsIndented => Indentation > 0;

        public int Indentation { get; set; }
        
        public bool IsSection 
        { 
            get {
                var clean = Content.Text.TrimEnd();
                var start = clean[0];
                for (int i = 1; i < clean.Length; i++)
                {
                    if (clean[i] != start)
                    {
                        return false;
                    }
                }

                return true; 
            }
        }

        public bool IsQuoted => Content.Text.StartsWith("> ");
        public ElementType ElementType => ElementType.Text;

        public TextArea(Content content)
        {
            Content = content;
        }

        public override string ToString()
        {
            return Content.Text;
        }
    }
}
