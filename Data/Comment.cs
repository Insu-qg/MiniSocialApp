namespace MiniSocialApp.Data;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Relation avec l’utilisateur
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Relation avec le post
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
}
