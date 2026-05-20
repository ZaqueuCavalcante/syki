using Syki.Back.Domain.Disciplines;

namespace Syki.Back.Features.Disciplines.GetDisciplines;

public static class GetDisciplinesMapper
{
    extension(Discipline discipline)
    {
        public GetDisciplinesItemOut ToGetDisciplinesItemOut()
        {
            return new()
            {
                Id = discipline.Id,
                Name = discipline.Name,
                Code = discipline.Code,
            };
        }
    }
}
