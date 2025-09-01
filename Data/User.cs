namespace MiniSocialApp.Data;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Bio { get; set; }
    public string? AvatarPath { get; set; }
}
