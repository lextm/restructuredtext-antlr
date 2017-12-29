using System.Collections.Generic;
using System.Linq;

namespace Lextm.ReStructuredText
{
    public class DefinitionList : IElement, IParent
    {
        public DefinitionList(IList<DefinitionListItem> definitionListItems)
        {
            foreach (var item in definitionListItems)
            {
                Add(item);
            }
        }

        public IList<DefinitionListItem> Items { get; } = new List<DefinitionListItem>();
        
        public void Add(IElement element, int level = 0)
        {
            
        }

        public ElementType TypeCode => ElementType.DefinitionList;
        public IList<ITextArea> TextAreas { get; }

        public IParent Parent { get; set; }

        public int Indentation => Items[0].Term.TextAreas[0].Indentation;

        public IElement Find(int line, int column)
        {
            foreach (var item in Items)
            {
                var result = item.Find(line, column);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public void Add(DefinitionListItem definitionListItem)
        {
            Items.Add(definitionListItem);
            definitionListItem.Parent = this;
        }
    }
}