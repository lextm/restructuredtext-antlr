using System.Collections.Generic;
using System.Linq;

namespace Lextm.ReStructuredText
{
    public class DefinitionList : IElement
    {
        public DefinitionList(IList<DefinitionListItem> definitionListItems)
        {
            foreach (var item in definitionListItems)
            {
                Items.Add(item);
                item.Parent = this;
            }
        }

        public IList<DefinitionListItem> Items { get; } = new List<DefinitionListItem>();
        
        public IParent Add(IElement element, int level = 0)
        {
            if (element is DefinitionListItem listItem)
            {
                Add(listItem);
                return listItem;
            }

            return Parent.Add(element);
        }

        public ElementType TypeCode => ElementType.DefinitionList;
        public IList<ITextArea> TextAreas => Items[0].Term.TextAreas;

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
            if (definitionListItem.Indentation == Indentation)
            {
                Items.Add(definitionListItem);
                definitionListItem.Parent = this;
            }
            else if (definitionListItem.Indentation > Indentation)
            {
                if (!(Items.Last().Definition.Elements.Last() is DefinitionList sublist))
                {
                    sublist = new DefinitionList(new List<DefinitionListItem> {definitionListItem});
                    sublist.Parent = this;
                    Items.Last().Definition.Elements.Add(sublist);
                }
                else
                {
                    sublist.Add(definitionListItem);
                }
            }
            else
            {
                Parent.Add(definitionListItem);
            }
        }
    }
}