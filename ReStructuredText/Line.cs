namespace ReStructuredText
{
    public class Line
    {
        public Text Text { get; }
        public bool IsIndented { get; internal set; }
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

        public Line(Text text)
        {
            Text = text;
        }
    }
}
