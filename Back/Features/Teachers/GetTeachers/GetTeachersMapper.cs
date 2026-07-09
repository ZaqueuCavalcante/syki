using Estud.Back.Domain.Teachers;

namespace Estud.Back.Features.Teachers.GetTeachers;

public static class GetTeachersMapper
{
    extension(EstudTeacher teacher)
    {
        public GetTeachersItemOut ToGetTeachersItemOut()
        {
            return new()
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.User!.Email!,
                Campi = teacher.Campi.Count,
                Disciplines = teacher.Disciplines.Count,
            };
        }
    }
}
