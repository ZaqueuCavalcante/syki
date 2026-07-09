namespace Estud.Back.Domain.Identity;

public class MagicLink
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }

    public EstudUser? User { get; set; }

    public MagicLink() {}

    public MagicLink(EstudUser user)
    {
        Id = Guid.NewGuid();
        User = user;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromHours(24));
    }

    public bool IsUsed() => UsedAt != null;
    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    public void Use()
    {
        UsedAt = DateTime.UtcNow;
    }
}
