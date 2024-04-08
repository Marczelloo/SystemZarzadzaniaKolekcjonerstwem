namespace SystemZarzadzaniaKolekcjonerstwem.Controls;

public partial class InfoRow : ContentView
{
	public static readonly BindableProperty InfoNameProperty =
        BindableProperty.Create(nameof(InfoName), typeof(string), typeof(InfoRow), string.Empty);
	public string InfoName
	{
        get => (string)GetValue(InfoNameProperty);
        set => SetValue(InfoNameProperty, value);
    }
	
	public static readonly BindableProperty InfoValueProperty =
        BindableProperty.Create(nameof(InfoValue), typeof(string), typeof(InfoRow), string.Empty);
	public string InfoValue
	{
        get => (string)GetValue(InfoValueProperty);
        set => SetValue(InfoValueProperty, value);
    }
	public InfoRow()
	{
		InitializeComponent();
	}
}