using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MiniSocialApp.Data;
using MiniSocialApp.Security;

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
                
                // TODO : ouvrir fenêtre principale (fil d’actualité)
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
