using System;

namespace ReStructuredText
{
    public class Line
    {
        public Text Text { get; }

        public bool IsIndented => Indentation > 0;

        public int Indentation { get; internal set; }
        
        public bool IsSection 
        { 
            get {
                var clean = Text.Content.TrimEnd();
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

        public char BulletChar
        {
            get
            {
                if (Text.Content.StartsWith("- ") || Text.Content.StartsWith("* ") || Text.Content.StartsWith("+ "))
                {
                    return Text.Content[0];
                }
                
                return char.MinValue;
            }
        }

        public Line(Text text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return Text.Content;
        }
    }
}
