namespace MiniSocialApp.Data;

public class Post
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
