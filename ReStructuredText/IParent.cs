namespace ReStructuredText
{
    public interface IParent
    {
        void Add(IElement element, int level = 0);
    }
}