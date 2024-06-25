using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Bindings.Models;
using Bindings.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Bindings.Views
{
    public partial class MainWindow : Window
    {
        // private bool isAscending = true;

        private int _clickCountPrice;

        // private int _clickCountAlphabet = 0;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        //конструктор для админа
        public MainWindow(bool isAdmin)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            Create.IsVisible = true;
            Delete.IsVisible = true;
            Add.IsVisible = true;
            OpenCart.IsVisible = true;

            // Edit.IsVisible = true;
        }

        //добавление товара в корзину
        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;

            if (viewModel != null && viewModel.Products.Count > 0 && viewModel.SelectedProducts.Count > 0)
            {
                var mainWindow = this;

                foreach (var product in viewModel.SelectedProducts)
                {
                    if (!viewModel.Cart.Any(p => p.Name == product.Name))
                    {
                        var cartProduct = new Product
                        {
                            Name = product.Name,
                            Price = product.Price,
                            Count = product.Count,
                            Manufacture = product.Manufacture,
                            ImagePath = product.ImagePath
                        };

                        viewModel.Cart.Add(cartProduct);
                    }
                    else
                    {
                        Error.Text = $"Продукт '{product.Name}' уже присутствует в корзине.";
                        await Task.Delay(3000);
                        Error.Text = string.Empty;
                    }
                }

                Korzina window1 = new Korzina(DataContext);

                window1.Closed += (s, args) => { mainWindow.Show(); };

                mainWindow.Hide();

                window1.Show();
            }
            else
            {
                Error.Text = "Выберите продукты перед открытием корзины.";
                await Task.Delay(3000);
                Error.Text = string.Empty;
            }
        }

        //удаление товара + MessageBoxManager
        private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = Prod.SelectedItems.OfType<Product>().ToList();

            if (selectedItems.Count == 0)
            {
                Error.Text = "Выберите, что хотите удалить!";
                await Task.Delay(3000);
                Error.Text = string.Empty;
                return;
            }

            var box = MessageBoxManager
                .GetMessageBoxStandard("Внимание!", "Вы действительно хотите удалить товар(ы)?", ButtonEnum.YesNo);
            var result = await box.ShowAsync();

            if (result == ButtonResult.No)
            {
                return;
            }

            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel != null)
            {
                foreach (var selectedItem in selectedItems)
                {
                    viewModel.Products.Remove(selectedItem);

                    for (int i = viewModel.Cart.Count - 1; i >= 0; i--)
                    {
                        if (viewModel.Cart[i].Name == selectedItem.Name)
                        {
                            viewModel.Cart.RemoveAt(i);
                        }
                    }
                }

                Prod.ItemsSource = viewModel.Products;
            }
        }

        // private async void EditButton_OnClick(object sender, RoutedEventArgs e)
        // {
        //     Product selectedProduct = (Product)Prod.SelectedItem;
        //
        //     if (selectedProduct != null)
        //     {
        //         var dialog = new EditProducts((MainWindowViewModel)DataContext, this, selectedProduct);
        //         dialog.SetProductFields(selectedProduct);
        //         dialog.ShowDialog(this);
        //     }
        //     else
        //     {
        //         Error.Text = "Выберите, что хотите изменить!";
        //         await Task.Delay(3000);
        //         Error.Text = string.Empty;
        //     }
        // }

        //по 2йному клику открывается редактирование
        private void Prod_OnDoubleTapped(object? sender, TappedEventArgs e)
        {
            Product selectedProduct = (Product)Prod.SelectedItem;

            var dialog = new EditProducts((MainWindowViewModel)DataContext, this, selectedProduct);
            dialog.SetProductFields(selectedProduct);
            dialog.ShowDialog(this);
        }

        //добавление товара
        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AddProducts((MainWindowViewModel)DataContext);
            dialog.ShowDialog(this);
        }

        //открытие корзины
        private async void OpenCartButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;

            if (viewModel != null && viewModel.Cart.Count > 0)
            {
                var mainWindow = this;

                Korzina window1 = new Korzina(DataContext);

                window1.Closed += (s, args) => { mainWindow.Show(); };

                mainWindow.Hide();

                window1.Show();
            }
            else
            {
                Error.Text = "Корзина пуста :(";
                await Task.Delay(3000);
                Error.Text = string.Empty;
            }
        }

        //поле поиск
        private void SearchTextBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel != null)
            {
                var filteredProducts = viewModel.SearchTextBox(Search.Text);

                Prod.ItemsSource = filteredProducts;
            }
        }


        //фильтрация по производителям
        private void SearchTextBox_OnKeyUp2(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel != null)
            {
                var selectedManufacturer = (string)((ComboBoxItem)ManufacturerComboBox.SelectedItem)?.Content;

                if (selectedManufacturer == "Все производители")
                {
                    selectedManufacturer = null;
                }

                var filteredProducts = viewModel.SearchTextBox2(selectedManufacturer);

                Prod.ItemsSource = filteredProducts;
            }
        }


        //сортировка по цене
        private void PriceButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var productsViewModel = (MainWindowViewModel)this.DataContext;

            _clickCountPrice++;

            switch (_clickCountPrice % 3)
            {
                case 0:
                    Prod.ItemsSource = productsViewModel?.Products;
                    PriceArrow.Text = "";
                    break;
                case 1:
                    Prod.ItemsSource = productsViewModel?.Products.OrderBy(p => p.Price);
                    PriceArrow.Text = "▲";
                    break;
                case 2:
                    Prod.ItemsSource = productsViewModel?.Products.OrderByDescending(p => p.Price);
                    PriceArrow.Text = "▼";
                    break;
            }
        }

        /*private void AlfavitButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var productsViewModel = (MainWindowViewModel)this.DataContext;

            clickCountAlphabet++;

            switch (clickCountAlphabet % 3)
            {
                case 0:
                    Prod.ItemsSource = productsViewModel.Products;
                    AlfavitArrow.Text = "";
                    break;
                case 1:
                    Prod.ItemsSource = productsViewModel.Products.OrderBy(p => p.Name);
                    AlfavitArrow.Text = "▲";
                    break;
                case 2:
                    Prod.ItemsSource = productsViewModel.Products.OrderByDescending(p => p.Name);
                    AlfavitArrow.Text = "▼";
                    break;
            }
        }*/


        //выйти на авторизацию
        private void LogOut(object? sender, RoutedEventArgs e)
        {
            var win = new Login();
            win.DataContext = new MainWindowViewModel();
            win.Show();
            Close();
        }
    }
}