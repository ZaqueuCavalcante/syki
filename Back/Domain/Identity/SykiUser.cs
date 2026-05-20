using Syki.Back.Domain.Institutions;

namespace Syki.Back.Domain.Identity;

public class SykiUser : IdentityUser<int>
{
    public int InstitutionId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ProfilePhoto { get; set; }

    public Institution? Institution { get; set; }

    public SykiUser() { }

    public SykiUser(
        Institution institution,
        string email
    ) {
        Id = 0;
        InstitutionId = 0;
        Name = email;
        UserName = email;
        Email = email;
        CreatedAt = DateTime.UtcNow;
        Institution = institution;
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
