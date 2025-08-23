using Syki.Back.Features.Academic.CreateCampus;

namespace Syki.Back.Features.Academic.GetCampi;

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
                Capacity = campus.Capacity,
                Students = students,
                Teachers = teachers,
                FillRate = campus.Capacity > 0 ? Math.Round(100M * (1M * students / (1M * campus.Capacity)), 2) : 0,
            };
        }
    }
}
