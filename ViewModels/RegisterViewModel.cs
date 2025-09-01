using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MiniSocialApp.Data;
using MiniSocialApp.Security;

namespace MiniSocialApp.ViewModels;

public class RegisterViewModel : INotifyPropertyChanged
{
    private string _username = "";
    private string _password = "";
    private string _bio = "";

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

    public string Bio
    {
        get => _bio;
        set { _bio = value; OnPropertyChanged(); }
    }

    public ICommand RegisterCommand { get; }

    public RegisterViewModel()
    {
        RegisterCommand = new RelayCommand(Register);
    }

    private void Register()
    {
        try
        {
            using var db = new AppDbContext();

            if (db.Users.Any(u => u.Username == Username))
            {
                MessageBox.Show("Ce nom d'utilisateur existe déjà !");
                return;
            }

            var user = new User
            {
                Username = Username,
                Password = PasswordHelper.HashPassword(Password),
                Bio = Bio
            };

            db.Users.Add(user);
            db.SaveChanges();

            MessageBox.Show("Compte créé avec succès !");
            Application.Current.Windows
                .OfType<Window>()
                .SingleOrDefault(w => w.DataContext == this)?
                .Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la création du compte : " + ex.Message);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
