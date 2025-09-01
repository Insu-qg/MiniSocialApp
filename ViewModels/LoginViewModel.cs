using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MiniSocialApp.Data;
using MiniSocialApp.Security;
using MiniSocialApp.Views;


namespace MiniSocialApp.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private string _username = "";
    private string _password = "";

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; }

    public SessionToken? CurrentSession { get; private set; }

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(Login);
    }

    private void Login()
    {
        try
        {
            using var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Username == Username);

            if (user != null && PasswordHelper.VerifyPassword(Password, user.Password))
            {
                CurrentSession = new SessionToken(user.Username);
                MessageBox.Show($"Bienvenue {user.Username} !\nToken : {CurrentSession.Token}");

                // Ouvrir fil d'actualité
                var feedWindow = new FeedView(CurrentSession);
                feedWindow.DataContext = new FeedViewModel(CurrentSession);
                feedWindow.Show();

                // Fermer Login
                Application.Current.Windows
                    .OfType<Window>()
                    .SingleOrDefault(w => w.DataContext == this)?
                    .Close();
            }
            else
            {
                MessageBox.Show("Identifiants incorrects");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la connexion : " + ex.Message);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
