using System.Windows;
using MiniSocialApp.ViewModels;

namespace MiniSocialApp.Views;

public partial class RegisterView : Window
{
    public RegisterView()
    {
        InitializeComponent();
        var vm = new RegisterViewModel();
        DataContext = vm;

        // Bind manuel pour PasswordBox
        PasswordBox.PasswordChanged += (s, e) =>
        {
            vm.Password = PasswordBox.Password;
        };
    }
}
