using Estud.Back.Domain.CourseCurriculums;

namespace Estud.Back.Features.CourseCurriculums.GetCourseCurriculums;

public static class GetCourseCurriculumsMapper
{
    extension(CourseCurriculum curriculum)
    {
        public GetCourseCurriculumsItemOut ToGetCourseCurriculumsItemOut()
        {
            return new()
            {
                Id = curriculum.Id,
                Name = curriculum.Name,
                CourseId = curriculum.CourseId,
                Course = curriculum.Course!.Name,
            };
        }
    }
}
