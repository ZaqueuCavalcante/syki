using Estud.Back.Domain.CourseOfferings;

namespace Estud.Back.Domain.Students;

/// <summary>
/// Vínculo entre um Aluno e uma Oferta de Curso.
/// <br/> <br/>
/// Um aluno pode ter múltiplos vínculos ativos simultaneamente (ex: graduação + pós-graduação).
/// </summary>
public class StudentCourseEnrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseOfferingId { get; set; }
    public DateTime EnrolledAt { get; set; }
    public DateTime? LeftAt { get; set; }

    public CourseOffering? CourseOffering { get; set; }

    private StudentCourseEnrollment() {}

    public StudentCourseEnrollment(int studentId, int courseOfferingId)
    {
        StudentId = studentId;
        CourseOfferingId = courseOfferingId;
        EnrolledAt = DateTime.UtcNow;
    }
}
