using Syki.Shared.CreateUser;
using Microsoft.AspNetCore.Identity;

namespace Syki.Back.CreateUser;

public class SykiUser : IdentityUser<Guid>
{
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public SykiUser(
        Guid institutionId,
        string name,
        string email
    ) {
        InstitutionId = institutionId;
        Name = name;
        UserName = email;
        Email = email;
        CreatedAt = DateTime.Now;
    }

    public CreateUserOut ToOut()
    {
        return new()
        {
            Id = Id,
            InstitutionId = InstitutionId,
            Nome = Name,
            Email = Email!,
        };
    }
}
