using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Bindings.ViewModels;

namespace Bindings.Views;

public partial class Login : Window
{
    public bool IsAdmin;

    public Login()
    {
        InitializeComponent();
    }

    private async void Login_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Text))
        {
            Oshibka.Text = "Заполните все поля!";
            await Task.Delay(3000);
            Oshibka.Text = string.Empty;
            return;
        }

        if (PasswordBox.Text == "0000")
        {
            IsAdmin = true;
        }

        var win = new MainWindow(IsAdmin);
        win.DataContext = new MainWindowViewModel();
        ((MainWindowViewModel)win.DataContext).Login = IsAdmin ? UsernameTextBox.Text : "гость";
        win.Show();
        Close();
    }

    private void ContinueAsGuest_Click(object sender, RoutedEventArgs e)
    {
        var win = new MainWindow();
        win.DataContext = new MainWindowViewModel();
        win.Show();
        Close();
    }
}