using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Bindings.Models;
using Bindings.ViewModels;

namespace Bindings.Views
{
    public partial class Korzina : Window
    {
        public Korzina(object dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;

            Update();

            ProductsChanges();

            var backButton = BackButton;

            backButton.Click += OnBackButtonClick;
        }
        
        //выйти с корзины
        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ProductsChanges()
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel != null)
            {
                viewModel.Products.CollectionChanged += (sender, args) => { Update(); };
            }
        }

        //айпдейт суммы
        private void Update()
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel != null)
            {
                var totalPrice = viewModel.Cart.Sum(product => product.Price * product.Count);
                Summa.Text = $"Общая сумма: {totalPrice} ₽";
            }
        }

        //удалить с корзины
        private void DeleteButton_OnClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var selectedItems = Cart.SelectedItems.OfType<Product>().ToList();
            var viewModel = DataContext as MainWindowViewModel;

            if (viewModel != null)
            {
                foreach (var selectedItem in selectedItems)
                {
                    viewModel.Cart.Remove(selectedItem);
                }

                Update();
            }
        }

        //увелечение и уменьшение цены по кнопкам
        // private void TextBox_KeyUp(object sender, KeyEventArgs e)
        // {
        //     if (e.Key == Key.Enter)
        //     {
        //         var textBox = (TextBox)sender;
        //         var product = (Product)textBox.DataContext;
        //
        //         if (int.TryParse(textBox.Text, out int count))
        //         {
        //             product.Count = count;
        //             Update();
        //         }
        //     }
        // }
        //
        // private void IncreaseCountButton_Click(object sender, RoutedEventArgs e)
        // {
        //     var button = (Button)sender;
        //     var product = (Product)button.DataContext;
        //
        //     product.Count++;
        //
        //     Update();
        // }
        //
        // private void DecreaseCountButton_Click(object sender, RoutedEventArgs e)
        // {
        //     var button = (Button)sender;
        //     var product = (Product)button.DataContext;
        //
        //     if (product.Count > 1)
        //     {
        //         product.Count--;
        //     }
        //
        //     Update();
        // }
    }
}