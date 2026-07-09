using Estud.Back.Domain.Campi;

namespace Estud.Back.Features.Campi.UpdateCampus;

public static class UpdateCampusMapper
{
    extension(Campus campus)
    {
        public UpdateCampusOut ToUpdateCampusOut()
        {
            return new()
            {
                Id = campus.Id,
                Name = campus.Name,
                City = campus.City,
                State = campus.State,
                Capacity = campus.Capacity,
            };
        }
    }
}
