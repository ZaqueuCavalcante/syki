namespace Syki.Back.Features.Cross.CreateUser;

public class SykiUser : IdentityUser<Guid>
{
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public SykiUser(
        Guid institutionId,
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

    public UserOut ToOut()
    {
        return new()
        {
            Id = Id,
            InstitutionId = InstitutionId,
            Name = Name,
            Email = Email!,
        };
    }
}
