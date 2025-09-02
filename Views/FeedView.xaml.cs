using System.Windows;
using System.Windows.Controls;
using MiniSocialApp.ViewModels; // Add this line if FeedViewModel is in the ViewModels namespace
using MiniSocialApp.Data; // ← Assure-toi d'ajouter ce using
using MiniSocialApp.Security;

namespace MiniSocialApp.Views;

    public partial class FeedView : Window
{
    public FeedView(SessionToken session)
    {
        InitializeComponent();
        DataContext = new FeedViewModel(session);

        Loaded += (s, e) => ScrollToBottom();
    }

    public void ScrollToBottom()
    {
        FeedScrollViewer?.ScrollToEnd();
    }
}

