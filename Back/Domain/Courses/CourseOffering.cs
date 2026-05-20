using Syki.Back.Domain.Campi;
using Syki.Back.Domain.Enums;
using Syki.Back.Domain.Periods;

namespace Syki.Back.Domain.Courses;

/// <summary>
/// Oferta de Curso
/// <br/> <br/>
/// Ao início de cada Período Acadêmico, uma Instituição pode ofertar um Curso em um Campus com um Currículo e um Turno.
/// </summary>
public class CourseOffering
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public int CampusId { get; set; }
    public int CourseId { get; set; }
    public int CourseCurriculumId { get; set; }
    public int AcademicPeriodId { get; set; }
    public CourseSession Session { get; set; }

    public Campus? Campus { get; set; }
    public Course? Course { get; set; }
    public CourseCurriculum? CourseCurriculum { get; set; }
    public AcademicPeriod? AcademicPeriod { get; set; }

    public CourseOffering() {}

    public CourseOffering(
        int institutionId,
        int campusId,
        int courseId,
        int courseCurriculumId,
        int academicPeriodId,
        CourseSession session
    ) {
        InstitutionId = institutionId;
        CampusId = campusId;
        CourseId = courseId;
        CourseCurriculumId = courseCurriculumId;
        AcademicPeriodId = academicPeriodId;
        Session = session;
    }
}
