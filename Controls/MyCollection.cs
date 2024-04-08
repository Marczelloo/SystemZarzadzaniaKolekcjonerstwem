using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemZarzadzaniaKolekcjonerstwem.Controls
{
    public class MyCollection
    {
        public string CollectionName { get; set; }
        public string CollectionImage { get; set; }

        public List<CollectionElement> collectionElements = new List<CollectionElement>();

        public MyCollection(string collectionName, string collectionImage)
        {
            CollectionName = collectionName;
            CollectionImage = collectionImage;
        }
        
        public MyCollection(string collectionName, string collectionImage, List<CollectionElement> collectionElements) : this(collectionName, collectionImage)
        {
            foreach(CollectionElement element in collectionElements)
            {
                CollectionElement newElement =  new CollectionElement();
                newElement.ElementName = element.ElementName;
                newElement.ElementImage = element.ElementImage;
                newElement.ElementDescription = element.ElementDescription;
                newElement.ElementDate = element.ElementDate;
                newElement.ElementValue = element.ElementValue;
                newElement.ElementSold = element.ElementSold;
                foreach(CollectionElementInfoRow infoRow in element.InfoRows)
                {
                    CollectionElementInfoRow newInfoRow = new CollectionElementInfoRow();
                    newInfoRow.InfoName = infoRow.InfoName;
                    newInfoRow.InfoValue = infoRow.InfoValue;
                    newElement.InfoRows.Add(newInfoRow);
                }
            }
        }

        public void AddElement(CollectionElement element)
        {
            collectionElements.Add(element);
        }

        public void RemoveElement(CollectionElement element)
        {
            collectionElements.Remove(element);
        }

        public void RemoveElement(int index)
        {
            collectionElements.RemoveAt(index);
        }

        public void ClearElements()
        {
            collectionElements.Clear();
        }

        public List<CollectionElement> GetElements()
        {
            return collectionElements;
        }
    }
}
