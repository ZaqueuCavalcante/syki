namespace Syki.Back.Domain;

public class ResetPassword
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UsedAt { get; set; }

    public ResetPassword(
        Guid userId,
        string token
    ) {
        Id = Guid.NewGuid();
        UserId = userId;
        Token = token;
        CreatedAt = DateTime.Now;
    }

    public void Use()
    {
        UsedAt = DateTime.Now;
    }
}
