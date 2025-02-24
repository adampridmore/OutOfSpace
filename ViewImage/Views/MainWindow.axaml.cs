using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace ViewImage.Views;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Image.BaseFolder = "/Users/davidbetteridge/OutOfSpace/Images";
        if (Directory.Exists(Image.BaseFolder))
        {
           DisplayFolder();
        }
    }

    
    private void FileSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (Files.SelectedItem is not null)
        {
            DisplayFile(Files.SelectedItem!.ToString()!);
        }
    }

    private async void ChangeFolderHandler(object? sender, RoutedEventArgs e)
    {
        try
        {
            var options = new FolderPickerOpenOptions
            {
                Title = "Select folder containing images",
                AllowMultiple = false,
                SuggestedStartLocation = await StorageProvider.TryGetFolderFromPathAsync(Image.BaseFolder)
            };
            var newFolder = await this.StorageProvider.OpenFolderPickerAsync(options);
            if (newFolder.Count == 0) return;
            Image.BaseFolder = newFolder.First().TryGetLocalPath()!;

            DisplayFolder();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void DisplayFolder()
    {
        var files = Directory.GetFiles(Image.BaseFolder, "*.sif")
            .Select(Path.GetFileName)
            .Order()
            .ToList();
        Files.ItemsSource = files;
        if (files.Count > 0)
            DisplayFile(files.First()!);
    }

    private void DisplayFile(string filename)
    {
        var bytes = Image.DisplayFile(filename);
        if (bytes is null)
        {
            Info.Text = "Invalid file"; 
            Hash.Text = "";
        }
        else
        {
            Info.Text = filename;
            Hash.Text = "Hash: " + ComputeHash(bytes);                
        }
    }

    private string ComputeHash(byte[] image)
    {
        var data = SHA256.HashData(image);
        var sBuilder = new StringBuilder();
        foreach (var t in data)
            sBuilder.Append(t.ToString("x2"));
        return sBuilder.ToString();
    }
}