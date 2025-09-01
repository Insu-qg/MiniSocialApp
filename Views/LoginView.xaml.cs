using System.Windows;
using System.Windows.Input;
using MiniSocialApp.ViewModels;

namespace MiniSocialApp.Views;

public partial class LoginView : Window
{
    private readonly LoginViewModel _vm;

    public LoginView()
    {
        InitializeComponent();
        _vm = new LoginViewModel();
        DataContext = _vm;

        // Bind manuel du PasswordBox
        PasswordBox.PasswordChanged += (s, e) =>
        {
            _vm.Password = PasswordBox.Password;
        };
    }

    private void RegisterLink_Click(object sender, MouseButtonEventArgs e)
    {
        var registerWindow = new RegisterView();
        registerWindow.ShowDialog(); // modal
    }
}
