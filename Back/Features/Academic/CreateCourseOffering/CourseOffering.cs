using Syki.Back.Features.Academic.CreateCourse;
using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Academic.CreateCourseCurriculum;

namespace Syki.Back.Features.Academic.CreateCourseOffering;

/// <summary>
/// Oferta de Curso
/// <br/> <br/>
/// Ao início de cada Período Acadêmico, uma Instituição pode ofertar determinado Curso.
/// </summary>
public class CourseOffering
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid CampusId { get; set; }
    public Campus Campus { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public Guid CourseCurriculumId { get; set; }
    public CourseCurriculum CourseCurriculum { get; set; }
    public string Period { get; set; }
    public Shift Shift { get; set; }

    public CourseOffering(
        Guid institutionId,
        Guid campusId,
        Guid courseId,
        Guid courseCurriculumId,
        string period,
        Shift shift
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        CampusId = campusId;
        CourseId = courseId;
        CourseCurriculumId = courseCurriculumId;
        Period = period;
        Shift = shift;
    }

    public CourseOfferingOut ToOut()
    {
        return new()
        {
            Id = Id,
            Campus = Campus.Name,
            Course = Course.Name,
            CourseCurriculumId = CourseCurriculumId,
            CourseCurriculum = CourseCurriculum.Name,
            Period = Period,
            Shift = Shift,
        };
    }
}
