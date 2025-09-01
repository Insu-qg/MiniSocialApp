using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MiniSocialApp.Data;
using MiniSocialApp.Security;

namespace MiniSocialApp.ViewModels;

public class FeedViewModel : INotifyPropertyChanged
{
    private readonly SessionToken _session;

    public ObservableCollection<PostDisplay> Posts { get; set; } = new();


    private string _newPostContent = "";
    public string NewPostContent
    {
        get => _newPostContent;
        set { _newPostContent = value; OnPropertyChanged(); }
    }

    public ICommand AddPostCommand { get; }
    public ICommand LikePostCommand { get; }
    public ICommand AddCommentCommand { get; }


    public FeedViewModel(SessionToken session)
    {
        _session = session;
        AddPostCommand = new RelayCommand(AddPost);
        LikePostCommand = new RelayCommand<PostDisplay>(LikePost);
        AddCommentCommand = new RelayCommand<Tuple<PostDisplay, string>>(AddComment);
        LoadPosts();
    }

    private void LoadPosts()
    {
        using var db = new AppDbContext();
        Posts.Clear();
        var posts = db.Posts
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new
            {
                p.Id,
                p.Content,
                Username = p.User.Username,
                p.CreatedAt,
                p.Likes, // Ajoute cette ligne
                Comments = p.Comments.Select(c => new CommentDisplay
                {
                    Content = c.Content,
                    User = new UserDisplay { Username = c.User.Username }
                }).ToList()
            })
            .ToList();

        foreach (var p in posts)
        {
            Posts.Add(new PostDisplay
            {
                Id = p.Id,
                Content = p.Content,
                Username = p.Username,
                CreatedAt = p.CreatedAt,
                Likes = p.Likes, // Ajoute cette ligne
                Comments = new ObservableCollection<CommentDisplay>(p.Comments)
            });
        }
    }

    private void AddPost()
    {
        if (string.IsNullOrWhiteSpace(NewPostContent))
            return;

        try
        {
            using var db = new AppDbContext();

            // Récupérer l'utilisateur connecté
            var user = db.Users.FirstOrDefault(u => u.Username == _session.Username);
            if (user == null)
            {
                MessageBox.Show("Utilisateur introuvable !");
                return;
            }

            var post = new Post
            {
                Content = NewPostContent,
                CreatedAt = DateTime.Now,
                UserId = user.Id
            };

            db.Posts.Add(post);
            db.SaveChanges();

            NewPostContent = "";
            LoadPosts();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de l'ajout du post : " + ex.Message);
        }
    }

    // Like d’un post
    private void LikePost(PostDisplay postDisplay)
    {
        using var db = new AppDbContext();
        var user = db.Users.FirstOrDefault(u => u.Username == _session.Username);
        var post = db.Posts.FirstOrDefault(p => p.Id == postDisplay.Id);

        if (user == null || post == null)
            return;

        // Vérifie si l'utilisateur a déjà liké ce post
        bool alreadyLiked = db.PostLikes.Any(pl => pl.PostId == post.Id && pl.UserId == user.Id);
        if (alreadyLiked)
            return;

        // Ajoute le like
        db.PostLikes.Add(new PostLike { PostId = post.Id, UserId = user.Id });
        post.Likes++; // Si tu veux garder le compteur pour la perf
        db.SaveChanges();
        LoadPosts();
    }

    // Ajouter un commentaire
    private void AddComment(Tuple<PostDisplay, string> param)
    {
        var postDisplay = param.Item1;
        var content = param.Item2;
        if (string.IsNullOrWhiteSpace(content)) return;

        using var db = new AppDbContext();
        var user = db.Users.FirstOrDefault(u => u.Username == _session.Username);
        var post = db.Posts.FirstOrDefault(p => p.Id == postDisplay.Id);

        if (user != null && post != null)
        {
            var comment = new Comment
            {
                Content = content,
                CreatedAt = DateTime.Now,
                UserId = user.Id,
                PostId = post.Id
            };
            db.Comments.Add(comment);
            db.SaveChanges();
            LoadPosts();
        }
    }





    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

// Classe d'affichage simplifiée pour binding
public class PostDisplay
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; }
    public ObservableCollection<CommentDisplay> Comments { get; set; } = new();
}

    public class CommentDisplay
    {
        public string Content { get; set; } = "";
        public UserDisplay User { get; set; } = new();
    }

    public class UserDisplay
    {
        public string Username { get; set; } = "";
    }
