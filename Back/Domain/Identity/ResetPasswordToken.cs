namespace Estud.Back.Domain.Identity;

public class ResetPasswordToken
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public int InstitutionId { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }

    public ResetPasswordToken(int userId, int institutionId, string token)
    {
        Id = Guid.CreateVersion7();
        UserId = userId;
        InstitutionId = institutionId;
        Token = token;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromMinutes(30));
    }

    public bool IsUsed() => UsedAt != null;
    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    public void Use()
    {
        UsedAt = DateTime.UtcNow;
    }
}
