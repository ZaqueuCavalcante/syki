namespace Estud.Back.Domain.Identity;

/// <summary>
/// Links a user to a social login provider (Google, Microsoft, etc.).
/// </summary>
public class UserSocialLogin
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public SocialLoginProvider Provider { get; set; }
    public string ProviderKey { get; set; }
    public string Email { get; set; }

    public DateTime CreatedAt { get; set; }

    public EstudUser? User { get; set; }

    public UserSocialLogin() { }

    public UserSocialLogin(
        int userId,
        SocialLoginProvider provider,
        string providerKey,
        string email)
    {
        UserId = userId;
        Provider = provider;
        ProviderKey = providerKey;
        Email = email.ToLowerInvariant();
        CreatedAt = DateTime.UtcNow;
    }
}
