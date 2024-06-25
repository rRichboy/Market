using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Bindings.Models;

namespace Bindings.ViewModels;

public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    public ObservableCollection<Product> Products { get; set; }
    public ObservableCollection<Product> SelectedProducts { get; set; }
    public ObservableCollection<Product> Cart { get; set; }
    
    
    //коллекции
    public MainWindowViewModel()
    {
        Products = new ObservableCollection<Product>();
        SelectedProducts = new ObservableCollection<Product>();
        Cart = new ObservableCollection<Product>();
    }
    
    //поле поиска
    public ObservableCollection<Product> SearchTextBox(string searchText)
    {
        var filteredProducts = new ObservableCollection<Product>(
            Products.Where(product => product.Name.Contains(searchText)));

        return filteredProducts;
    }
    
    //фильтрация по производителям
    public ObservableCollection<Product> SearchTextBox2(string selectedManufacturer)
    {
        var filteredProducts = new ObservableCollection<Product>(
            Products.Where(product => 
                selectedManufacturer == null || product.Manufacture == selectedManufacturer)
        );

        return filteredProducts;
    }
    
    //вывод логина в маине
        
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    private string _login;

    public string Login
    {
        get
        {
            if (string.IsNullOrEmpty(_login))
            {
                return "гость";
            }
            else
            {
                return _login;
            }
        }
        set
        {
            _login = value;
            OnPropertyChanged();
        }
    }
}