using Estud.Back.Domain.Campi;

namespace Estud.Back.Features.Campi.GetCampi;

public static class GetCampiMapper
{
    extension(Campus campus)
    {
        public GetCampiItemOut ToGetCampiItemOut(int students, int teachers)
        {
            return new()
            {
                Id = campus.Id,
                Name = campus.Name,
                City = campus.City,
                State = campus.State,
                Students = students,
                Teachers = teachers,
            };
        }
    }
}
