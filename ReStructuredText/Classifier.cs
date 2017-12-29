using System.Collections.Generic;
using System.Linq;

namespace Lextm.ReStructuredText
{
    public class Classifier
    {
        public IList<ITextArea> TextAreas;

        public Classifier(List<ITextArea> textAreas)
        {
            textAreas.Last().Content.RemoveEnd();
            textAreas.First().Content.RemoveStart();
            TextAreas = new List<ITextArea>();
            foreach (var item in textAreas)
            {
                if (string.IsNullOrWhiteSpace(item.Content.Text))
                {
                    continue;
                }

                TextAreas.Add(item);
            }
        }
    }
}