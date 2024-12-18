namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public class ResetPasswordToken : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UsedAt { get; set; }

    public ResetPasswordToken(
        Guid userId,
        string token
    ) {
        Id = Guid.NewGuid();
        UserId = userId;
        Token = token;
        CreatedAt = DateTime.Now;

        AddDomainEvent(new ResetPasswordTokenCreatedDomainEvent(userId));
    }

    public void Use()
    {
        UsedAt = DateTime.Now;
    }
}
