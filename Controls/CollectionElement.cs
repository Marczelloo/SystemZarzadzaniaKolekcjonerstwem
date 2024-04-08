using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemZarzadzaniaKolekcjonerstwem.Controls
{
    public partial class CollectionElement
    {
        public string ElementName 
        { 
            get => (string)GetValue(ElementNameProperty); 
            set => SetValue(ElementNameProperty, value); 
        }
        public string ElementImage 
        { 
            get => (string)GetValue(ElementImageProperty); 
            set => SetValue(ElementImageProperty, value); 
        }
        public string ElementDescription 
        { 
            get => (string)GetValue(ElementDescriptionProperty); 
            set => SetValue(ElementDescriptionProperty, value); 
        }
        public float ElementValue 
        { 
            get => (float)GetValue(ElementValueProperty);
            set
            {
                if(ElementValue != value)
                {
                    SetValue(ElementValueProperty, value);
                }
            }
         }
        public string ElementDate 
        { 
            get => (string)GetValue(ElementDateProperty); 
            set => SetValue(ElementDateProperty, value); 
        }

        public bool ElementSold 
        { 
            get => (bool)GetValue(ElementSoldProperty); 
            set => SetValue(ElementSoldProperty, value); 
        }

        public List<CollectionElementInfoRow> InfoRows = new List<CollectionElementInfoRow>();

        public CollectionElement(string elementName, string elementImage, string elementDescription, float elementValue, string elementDate, bool elementSold)
        {
            InitializeComponent();
            ElementName = elementName;
            ElementImage = elementImage;
            ElementDescription = elementDescription;
            ElementValue = elementValue;
            ElementDate = elementDate;
            ElementSold = elementSold;
            PropertyChanged += OnPropertyChanged;
            if (ElementSold)
            {
                CollectionElementCard.Opacity = 0.5;
            }
            else
            {
                CollectionElementCard.Opacity = 1;
            }

        }

        public CollectionElement(string elementName, string elementImage, string elementDescription, float elementValue, string elementDate, bool elementSold, List<CollectionElementInfoRow> infoRows) : this(elementName, elementImage, elementDescription, elementValue, elementDate, elementSold)
        {
            InitializeComponent();
            InfoRows = infoRows;
            PropertyChanged += OnPropertyChanged;
        }
    }
}
