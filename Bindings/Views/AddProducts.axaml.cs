using System.Collections.Generic;
using Avalonia.Controls;
using Bindings.Models;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Bindings.ViewModels;

namespace Bindings.Views
{
    public partial class AddProducts : Window
    {
        private MainWindowViewModel viewModel;
        private Bitmap _imagePath;

        public AddProducts(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
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

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nm.Text) || string.IsNullOrWhiteSpace(pr.Text) || string.IsNullOrWhiteSpace(ct.Text))
            {
                Error.Text = "Заполните все поля!";
                await Task.Delay(3000);
                Error.Text = string.Empty;
                return;
            }

            if (_imagePath == null)
            {
                _imagePath = new Bitmap("C:\\Users\\glkho\\RiderProjects\\Bindings\\Bindings\\Assets\\unnamed.png");
            }

            if (!int.TryParse(pr.Text, out int price))
            {
                Error.Text = "Введите корректную цену!";
                await Task.Delay(3000);
                Error.Text = string.Empty;
                return;
            }

            if (!int.TryParse(ct.Text, out int count))
            {
                Error.Text = "Введите корректное количество!";
                await Task.Delay(3000);
                Error.Text = string.Empty;
                return;
            }

            var newProduct = new Product
            {
                Name = nm.Text,
                Price = price,
                Count = count,
                ImagePath = _imagePath,
                Manufacture = (string)((ComboBoxItem)ManufacturerComboBox.SelectedItem).Content
            };

            if (viewModel.Products.Any(p => p.Name == newProduct.Name))
            {
                Error.Text = $"Товар с именем '{newProduct.Name}' уже существует.";
                await Task.Delay(3000);
                Error.Text = string.Empty;
                return;
            }

            viewModel.Products.Add(newProduct);

            nm.Text = string.Empty;
            pr.Text = string.Empty;
            ct.Text = string.Empty;
            Close();
        }
    }
}
