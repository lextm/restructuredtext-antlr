namespace ReStructuredText
{
    public class Line
    {
        public Text Text { get; }
        public bool IsIndented { get; internal set; }

        public Line(Text text)
        {
            Text = text;
        }
    }
}
