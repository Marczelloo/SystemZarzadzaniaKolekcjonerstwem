using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SystemZarzadzaniaKolekcjonerstwem.Pages;

namespace SystemZarzadzaniaKolekcjonerstwem.Controls;

public partial class CollectionElement : ContentView
{
    public event EventHandler RemoveElementClicked;
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(ElementValue))
        {
            OnPropertyChanged(nameof(ElementValueText));
        }

		if(propertyName == nameof(ElementSold))
		{
			OnPropertyChanged(nameof(ElementSoldText));
            if (ElementSold)
            {
                CollectionElementCard.Opacity = 0.5;
            }
            else
            {
                CollectionElementCard.Opacity = 1;
            }
        }
    }
    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
		if (e.PropertyName == "ElementValue")
		{
			OnPropertyChanged(nameof(ElementValueText));
		}

		if (e.PropertyName == "ElementSold")
		{
			OnPropertyChanged(nameof(ElementSoldText));
			if(ElementSold)
			{
                CollectionElementCard.Opacity = 0.5;
            }
            else
			{
				CollectionElementCard.Opacity = 1;
            }
		}
    }

    public static readonly BindableProperty ElementNameProperty = 
        BindableProperty.Create(nameof(ElementName), typeof(string), typeof(CollectionElement), string.Empty);

	public static readonly BindableProperty ElementImageProperty =
		BindableProperty.Create(nameof(ElementImage), typeof(string), typeof(CollectionElement), string.Empty);

	public static readonly BindableProperty ElementDescriptionProperty =
		BindableProperty.Create(nameof(ElementDescription), typeof(string), typeof(CollectionElement), string.Empty);

	public static readonly BindableProperty ElementValueProperty =
		BindableProperty.Create(nameof(ElementValue), typeof(float), typeof(CollectionElement), 0.0f);

	public static readonly BindableProperty ElementDateProperty =
		BindableProperty.Create(nameof(ElementDate), typeof(string), typeof(CollectionElement), string.Empty);

	public static readonly BindableProperty ElementSoldProperty =
		BindableProperty.Create(nameof(ElementSold), typeof(bool), typeof(CollectionElement), false);

	public static readonly BindableProperty ElementSoldTextProperty =
        BindableProperty.Create(nameof(ElementSoldText), typeof(string), typeof(CollectionElement), string.Empty);

	public static readonly BindableProperty ElementValueTextProperty =
		BindableProperty.Create(nameof(ElementValueText), typeof(string), typeof(CollectionElement), string.Empty);

	private CollectionPage collectionPage;

	public CollectionPage _CollectionPage
	{
        get => collectionPage;
        set => collectionPage = value;
    }
	public string ElementSoldText
	{
		get => ElementSold ? "Sold" : "Not sold";
	}

	public string ElementValueText
	{
		get
		{
			return ElementValue.ToString("F2") + " EUR"; 
		}
        set
        {
            if (ElementValueText != value)
            {
                ElementValueText = value;
            }
        }
    }

    public CollectionElement()
	{
		InitializeComponent();
        PropertyChanged += OnPropertyChanged;
    }

    private void Edit_Clicked(object sender, EventArgs e)
	{
		CollectionElement elementToEdit = new CollectionElement();
		elementToEdit.ElementName = ElementName;
		elementToEdit.ElementDescription = ElementDescription;
		elementToEdit.ElementImage = ElementImage;
		elementToEdit.ElementValue = ElementValue;
		elementToEdit.ElementDate = ElementDate;
		elementToEdit.ElementSold = ElementSold;
		foreach (CollectionElementInfoRow infoRow in InfoRows)
		{
            CollectionElementInfoRow newInfoRow = new CollectionElementInfoRow();
            newInfoRow.InfoName = infoRow.InfoName;
            newInfoRow.InfoValue = infoRow.InfoValue;
            elementToEdit.InfoRows.Add(newInfoRow);
        }
		Navigation.PushAsync(new AddCollectionElementPage(collectionPage, elementToEdit));
	}

	private void Delete_Clicked(object sender, EventArgs e)
	{
		RemoveElementClicked?.Invoke(this, EventArgs.Empty);
    }

	   public void AddInfoRows()
    {
        try
        {

            foreach(var element in InfoLayout.Children)
            {
                if(element is InfoRow)
                {
                    InfoLayout.Children.Remove(element);
                }
            }

            foreach (CollectionElementInfoRow row in InfoRows)
            {
                InfoRow infoRow = new InfoRow();
                infoRow.InfoName = row.InfoName;
                infoRow.InfoValue = row.InfoValue;

                InfoLayout.Children.Add(infoRow);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
}