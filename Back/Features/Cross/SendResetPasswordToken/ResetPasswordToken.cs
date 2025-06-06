namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public class ResetPasswordToken : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid InstitutionId { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UsedAt { get; set; }

    public ResetPasswordToken(Guid userId, Guid institutionId, string token)
    {
        Id = Guid.CreateVersion7();
        UserId = userId;
        InstitutionId = institutionId;
        Token = token;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new ResetPasswordTokenCreatedDomainEvent(userId));
    }

    public void Use()
    {
        UsedAt = DateTime.UtcNow;
    }
}
