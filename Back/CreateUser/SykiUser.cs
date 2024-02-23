using Syki.Shared;
using Microsoft.AspNetCore.Identity;

namespace Syki.Back.CreateUser;

public class SykiUser : IdentityUser<Guid>
{
    public Guid FaculdadeId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public SykiUser(
        Guid faculdadeId,
        string name,
        string email
    ) {
        FaculdadeId = faculdadeId;
        Name = name;
        UserName = email;
        Email = email;
        CreatedAt = DateTime.Now;
    }

    public UserOut ToOut()
    {
        return new()
        {
            Id = Id,
            FaculdadeId = FaculdadeId,
            Nome = Name,
            Email = Email!,
        };
    }
}
