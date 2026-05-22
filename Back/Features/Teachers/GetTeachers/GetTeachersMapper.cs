using Syki.Back.Domain.Teachers;

namespace Syki.Back.Features.Teachers.GetTeachers;

public static class GetTeachersMapper
{
    extension(SykiTeacher teacher)
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
