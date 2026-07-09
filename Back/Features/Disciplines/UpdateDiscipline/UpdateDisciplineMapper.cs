using Estud.Back.Domain.Disciplines;

namespace Estud.Back.Features.Disciplines.UpdateDiscipline;

public static class UpdateDisciplineMapper
{
    extension(Discipline discipline)
    {
        public UpdateDisciplineOut ToUpdateDisciplineOut()
        {
            return new()
            {
                Id = discipline.Id,
                Name = discipline.Name,
            };
        }
    }
}
