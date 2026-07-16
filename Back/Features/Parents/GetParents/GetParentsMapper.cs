using Estud.Back.Domain.Parents;

namespace Estud.Back.Features.Parents.GetParents;

public static class GetParentsMapper
{
    extension(EstudParent parent)
    {
        public GetParentsItemOut ToGetParentsItemOut()
        {
            return new()
            {
                Id = parent.Id,
                Name = parent.Name,
                Email = parent.User!.Email!,
                PhoneNumber = parent.User!.PhoneNumber,
            };
        }
    }
}
