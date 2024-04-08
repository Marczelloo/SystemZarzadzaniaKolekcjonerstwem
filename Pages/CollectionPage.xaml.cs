namespace SystemZarzadzaniaKolekcjonerstwem.Pages;

using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Text;
using SystemZarzadzaniaKolekcjonerstwem.Controls;
public partial class CollectionPage : ContentPage
{
	public string CollectionName { get; set; }
    public string CollectionImage { get; set; }
    public List<CollectionElement> CollectionElements { get; set; } = new List<CollectionElement>();
	public CollectionPage()
	{
        Debug.WriteLine("CollectionPage constructor");
		InitializeComponent();
	}

    override protected void OnAppearing()
    {
        base.OnAppearing();
        if(CollectionElements.Count > 0)
        {
            RefreshCollectionElements();
        }
    }

    override protected void OnDisappearing()
    {
        base.OnDisappearing();
    }

	public CollectionPage(string collectionName, string collectionImage, List<CollectionElement> collectionElements)
	{
        try
        {
            InitializeComponent();
            CollectionName = collectionName;
            CollectionImage = collectionImage;
            foreach(CollectionElement element in collectionElements)
            {
                CollectionElement newElement = new CollectionElement(element.ElementName, element.ElementImage, element.ElementDescription, element.ElementValue, element.ElementDate, element.ElementSold);
                foreach(CollectionElementInfoRow infoRow in element.InfoRows)
                {
                    CollectionElementInfoRow newInfoRow = new CollectionElementInfoRow();
                    newInfoRow.InfoName = infoRow.InfoName;
                    newInfoRow.InfoValue = infoRow.InfoValue;
                    newElement.InfoRows.Add(newInfoRow);
                }
                CollectionElements.Add(newElement);
            }

            foreach(CollectionElement element in CollectionElements)
            {
                element._CollectionPage = this;
            } 
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    private void RefreshCollectionElements()
    {
        Border AddElement = elementsLayout.FindByName<Border>("AddElementCard");
        elementsLayout.Children.Clear();
        foreach(CollectionElement element in CollectionElements)
        {
            if (!element.ElementSold)
            {
                element.RemoveElementClicked += OnRemoveElementClicked;
                elementsLayout.Children.Add(element);
                element.AddInfoRows();
            }
        }
        foreach(CollectionElement element in CollectionElements)
        {
            if(element.ElementSold)
            {
                element.RemoveElementClicked += OnRemoveElementClicked;
                elementsLayout.Children.Add(element);
                element.AddInfoRows();
            }
        }
        elementsLayout.Children.Add(AddElement);

    }

    private void OnRemoveElementClicked(object sender, EventArgs e)
    {
        if(sender is CollectionElement elementToRemove)
        {
            File.Delete(elementToRemove.ElementImage);
            CollectionElements.Remove(elementToRemove);
            RefreshCollectionElements();
        }
        SaveCollection();
    }

    private async void AddElement_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddCollectionElementPage(this));
        RefreshCollectionElements();
    }

    public void SaveCollection()
    {
        Debug.WriteLine("Saving collection");
        string appDataPath = FileSystem.AppDataDirectory;
        string filePath = Path.Combine(appDataPath, "collections.txt");

        string[] lines = File.ReadAllLines(filePath);
        string data = CollectionName + "," + CollectionImage;
        foreach (CollectionElement element in CollectionElements)
        {
            Debug.WriteLine("Saving element " + element.ElementName + " " + element.ElementDescription);
            data += ";" + element.ElementName + "," + element.ElementDescription + "," + element.ElementImage + "," + element.ElementValue + "," + element.ElementDate + "," + element.ElementSold;
            foreach (CollectionElementInfoRow infoRow in element.InfoRows)
            {
                data += "|" + infoRow.InfoName + "," + infoRow.InfoValue;
            }
        }

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith(CollectionName))
            {
                lines[i] = data;
                break;
            }
        }

        foreach(string line in lines)
        {
            Debug.WriteLine(line);
        }

        File.WriteAllLines(filePath, lines);
    }

    private void GenerateSummary_Clicked(object sender, EventArgs e)
    {
        int numberOfObjects = 0;
        int numberOfSoldObjects = 0;
        int numberOfUnsoldObjects = 0;
        double totalValue = 0;
        foreach(CollectionElement element in CollectionElements)
        {
            numberOfObjects++;
            totalValue += element.ElementValue;
            if(element.ElementSold)
            {
                numberOfSoldObjects++;
            }
            else
            {
                numberOfUnsoldObjects++;
            }
        }
        DisplayAlert("Collection summary", $"Number of objects: {numberOfObjects}\nNumber of sold objects: {numberOfSoldObjects}\nNumber of unsold objects: {numberOfUnsoldObjects}\nTotal value of collection: {totalValue.ToString("F2")} EUR", "OK");
    }

    private async void ExportElements_Clicked(object sender, EventArgs e)
    {
        await SaveFile(CancellationToken.None);
    }

    private async Task SaveFile(CancellationToken token)
    {
        string data = string.Empty;
        foreach (CollectionElement element in CollectionElements)
        {
            data += element.ElementName + "," + element.ElementDescription + "," + element.ElementImage + "," + element.ElementValue + "," + element.ElementDate + "," + element.ElementSold;
            foreach (CollectionElementInfoRow infoRow in element.InfoRows)
            {
                data += "|" + infoRow.InfoName + "," + infoRow.InfoValue;
            }
            data += Environment.NewLine;
        }

        using var stream = new MemoryStream(Encoding.Default.GetBytes(data));
        var fileSaverResult = await FileSaver.Default.SaveAsync("Exported " + CollectionName + ".txt", stream, token);
        if (fileSaverResult.IsSuccessful)
        {
            await DisplayAlert("Success", "The file was exported successfully", "OK");
        }
        else
        {
            await DisplayAlert("Error", "There was an error while exporting file", "OK");
        }
    }

    private async void ImportElements_Clicked(object sender, EventArgs e)
    {
        try
        {
            FileResult fileResult = await FilePicker.PickAsync();

            if (fileResult != null && !string.IsNullOrEmpty(fileResult.FullPath))
            {
                List<CollectionElement> collectionElements = new List<CollectionElement>();

                string[] lines = File.ReadAllLines(fileResult.FullPath);

                foreach (string line in lines)
                {
                    string[] infoRows = line.Split("|");

                    string[] elementInfo = infoRows[0].Split(",");
                    string elementName = elementInfo[0];
                    string elementDescription = elementInfo[1];
                    string elementImage = elementInfo[2];
                    float elementValue = float.Parse(elementInfo[3]);
                    string elementDate = elementInfo[4];
                    bool elementSold = bool.Parse(elementInfo[5]);
                    CollectionElement collectionElement = new CollectionElement(elementName, elementImage, elementDescription, elementValue, elementDate, elementSold);
                    collectionElement._CollectionPage = this;

                    foreach (string row in infoRows.Skip(1))
                    {
                        string[] infoRow = row.Split(",");
                        string infoName = infoRow[0];
                        string infoValue = infoRow[1];
                        CollectionElementInfoRow info = new CollectionElementInfoRow(infoName, infoValue);
                        collectionElement.InfoRows.Add(info);
                    }

                    collectionElements.Add(collectionElement);
                }

                foreach (CollectionElement element in collectionElements)
                {
                    CollectionElements.Add(element);
                }

                RefreshCollectionElements();
                SaveCollection();
            }
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
}