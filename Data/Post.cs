namespace MiniSocialApp.Data;
public class Post
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Relation avec l’utilisateur
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Likes et commentaires
    public int Likes { get; set; } = 0;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
