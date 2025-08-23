using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Academic.CreateTeacher;

namespace Syki.Back.Features.Academic.GetAcademicTeacher;

public static class GetAcademicTeacherMapper
{
    extension(SykiTeacher teacher)
    {
        public GetAcademicTeacherOut ToGetAcademicTeacherOut()
        {
            return new()
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Campi = teacher.Campi.ConvertAll(x => x.ToGetAcademicTeacherCampusOut()),
                Disciplines = teacher.Disciplines.ConvertAll(x => x.ToGetAcademicTeacherDisciplineOut()),
            };
        }
    }

    extension(Campus campus)
    {
        public GetAcademicTeacherCampusOut ToGetAcademicTeacherCampusOut()
        {
            return new()
            {
                Id = campus.Id,
                Name = $"{campus.Name} ({campus.City} - {campus.State})",
            };
        }
    }
}
