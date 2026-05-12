namespace Syki.Back.Features.Academic.CreateCampus;

public static class CreateCampusMapper
{
    extension(Campus campus)
    {
        public CreateCampusOut ToCreateCampusOut()
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
