using Syki.Shared;
using Microsoft.AspNetCore.Identity;

namespace Syki.Back.Domain;

public class SykiUser : IdentityUser<Guid>
{
    public Guid FaculdadeId { get; set; }

    public string Name { get; set; }

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
