namespace Syki.Back.Domain.Identity;

public class SykiUser : IdentityUser<int>
{
    public int InstitutionId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ProfilePhoto { get; set; }

    public SykiUser(
        int institutionId,
        string name,
        string email,
        string? phoneNumber = null
    ) {
        InstitutionId = institutionId;
        Name = name;
        UserName = email;
        Email = email;
        PhoneNumber = phoneNumber;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string profilePhoto)
    {
        Name = name;
        ProfilePhoto = profilePhoto;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
    }
}
