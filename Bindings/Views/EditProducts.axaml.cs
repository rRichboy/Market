using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Bindings.Models;
using Bindings.ViewModels;

namespace Bindings.Views
{
    public partial class EditProducts : Window
    {
        private TextBox _nameTextBox;
        private TextBox _priceTextBox;
        private TextBox _countTextBox;
        private MainWindow _mainWindow;
        private Product _selectedProduct;
        private Bitmap _imagePath;
        private MainWindowViewModel viewModel;


        public EditProducts(MainWindowViewModel viewModel, MainWindow mainWindow, Product selectedProduct)
        {
            InitializeComponent();

            _nameTextBox = nm;
            _priceTextBox = pr;
            _countTextBox = ct;
            _mainWindow = mainWindow;
            _selectedProduct = selectedProduct;
            DataContext = viewModel;

            Closed += OnClosed;
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            _mainWindow.DataContext = null;
            _mainWindow.DataContext = DataContext;
            _mainWindow.Show();
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct != null)
            {
                _selectedProduct.Name = _nameTextBox.Text;
                _selectedProduct.Price = int.Parse(_priceTextBox.Text);
                _selectedProduct.Count = int.Parse(_countTextBox.Text);
                _selectedProduct.ImagePath = _imagePath;
                _selectedProduct.Manufacture = (string)((ComboBoxItem)ManufacturerComboBox.SelectedItem).Content;

                Close();
            }
        }

        public void SetProductFields(Product selectedProduct)
        {
            _nameTextBox.Text = selectedProduct.Name;
            _priceTextBox.Text = selectedProduct.Price.ToString();
            _countTextBox.Text = selectedProduct.Count.ToString();
            _imagePath = selectedProduct.ImagePath;

            foreach (ComboBoxItem item in ManufacturerComboBox.Items)
            {
                if (item.Content.ToString() == selectedProduct.Manufacture)
                {
                    ManufacturerComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private async void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Title = "Выберите изображение",
                Directory = "C:\\Users\\glkho\\RiderProjects\\Bindings\\Bindings\\Assets",
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "Images", Extensions = { "jpg", "png", "jpeg", "ico" } }
                }
            };

            var result = await fileDialog.ShowAsync(this);

            if (result != null && result.Length > 0)
            {
                var imagePath = result[0];
                var bitmap = new Bitmap(imagePath);
                _imagePath = bitmap;
            }
        }
    }
}