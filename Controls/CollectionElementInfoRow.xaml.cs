using System.Diagnostics;

namespace SystemZarzadzaniaKolekcjonerstwem.Controls;

public partial class CollectionElementInfoRow : ContentView
{
    public static readonly BindableProperty InfoNameProperty =
        BindableProperty.Create(nameof(InfoName), typeof(string), typeof(CollectionElementInfoRow), string.Empty);

    public static readonly BindableProperty InfoValueProperty =
        BindableProperty.Create(nameof(InfoValue), typeof(string), typeof(CollectionElementInfoRow), string.Empty);
    public string InfoName 
    { 
        get => (string)GetValue(InfoNameProperty);
        set => SetValue(InfoNameProperty, value); 
    }
    public string InfoValue 
    { 
        get => (string)GetValue(InfoValueProperty);
        set => SetValue(InfoValueProperty, value); 
    }

    public event EventHandler RemoveInfoRowClicked;

    public CollectionElementInfoRow()
	{
		InitializeComponent();
	}

    public CollectionElementInfoRow(string infoName)
    {
        InitializeComponent();
        InfoName = infoName;
    }
    public CollectionElementInfoRow(string infoName, string infoValue)
    {
        InitializeComponent();
        InfoName = infoName;
        InfoValue = infoValue;
    }

    private void RemoveInfoRow_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("RemoveInfoRow_Clicked");
        RemoveInfoRowClicked?.Invoke(this, EventArgs.Empty);
    }

    private void InfoValueEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        InfoValue = e.NewTextValue;
    }
}