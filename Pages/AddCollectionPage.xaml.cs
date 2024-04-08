using System.Diagnostics;
using SystemZarzadzaniaKolekcjonerstwem.Controls;

namespace SystemZarzadzaniaKolekcjonerstwem.Pages;

public partial class AddCollectionPage : ContentPage
{
    public string CollectionName
    {
        get; set;
    }
    public string CollectionImage
    {
        get; set;
    }

    public List<CollectionElement> CollectionElements
    {
        get; set;
    }

    private bool isEditing = false;

    public AddCollectionPage()
    {
        InitializeComponent();
    }

    public AddCollectionPage(string collectionName, string collectionImage, List<CollectionElement> elements)
    {
        InitializeComponent();
        CollectionName = collectionName;
        CollectionImage = collectionImage;
        CollectionElements = elements;
        isEditing = true;
        CollectionNameEntry.Text = CollectionName;
        string fileName = Path.GetFileName(CollectionImage);
        imageLabel.Text = "Image selected: " + fileName;
        addButton.Text = "Edit collection";
        elementImage.Source = CollectionImage;
    }

    private void AddCollection_Clicked(object sender, EventArgs e)
    {
        if (isEditing)
        {
            Edit_Collection();
        }
        else
        {
            Add_Collection();
        }
    }

    private void Edit_Collection()
    {
        string oldCollectionName = CollectionName;
        string oldCollectionImage = CollectionImage;

        CollectionName = CollectionNameEntry.Text;

        string appDataPath = FileSystem.AppDataDirectory;
        string filePath = Path.Combine(appDataPath, "collections.txt");

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }

        string[] lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            if (line.Contains(oldCollectionName) && line.Contains(oldCollectionImage))
            {
                lines = lines.Where(val => val != line).ToArray();
            }
        }

        string data = CollectionName + "," + CollectionImage;
        CollectionElements.ForEach(element =>
        {
            data += ";";
            data += element.ElementName + "," + element.ElementDescription + "," + element.ElementImage + "," + element.ElementValue + "," + element.ElementDate + element.ElementSold.ToString();
            element.InfoRows.ForEach(row =>
            {
                data += "|";
                data += row.InfoName + "," + row.InfoValue;
            });
        });

        lines = lines.Append(data).ToArray();

        File.WriteAllText(filePath, string.Empty);

        foreach (var item in lines)
        {
            File.AppendAllText(filePath, item + Environment.NewLine);
        }

        Navigation.PopAsync();
        //Navigation.PushAsync(new MainPage());
    }

    private void Add_Collection()
    {
        try
        {
            CollectionName = CollectionNameEntry.Text;

            if (string.IsNullOrEmpty(CollectionName))
            {
                DisplayAlert("Error", "Collection name cannot be empty", "OK");
                return;
            }

            if (string.IsNullOrEmpty(CollectionImage))
            {
                DisplayAlert("Error", "Collection image cannot be empty", "OK");
                return;
            }

            string appDataPath = FileSystem.AppDataDirectory;
            string imagePath = CollectionImage;
            string newFileName = CollectionName + ".png";
            string newFilePath = Path.Combine(appDataPath, newFileName);
            File.Copy(imagePath, newFilePath);
            CollectionImage = newFilePath;

            string filePath = Path.Combine(appDataPath, "collections.txt");

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            MyCollection collection = new MyCollection(CollectionName, CollectionImage);

            string data = CollectionName + "," + CollectionImage;

            File.AppendAllText(filePath, data + Environment.NewLine);

            Navigation.PopAsync();
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    private async void SelectImage_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick an image",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            elementImage.Source = result.FullPath;
            CollectionImage = result.FullPath;
            imageLabel.Text = "Image selected: " + result.FileName;
        }
    }
}