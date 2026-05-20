using Syki.Back.Domain.Campi;
using Syki.Back.Domain.Courses;
using Syki.Back.Domain.Periods;
using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Students;
using Syki.Back.Domain.Teachers;
using Syki.Back.Domain.Disciplines;

namespace Syki.Back.Domain.Institutions;

public class Institution
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Campus> Campi { get; set; }
    public List<Course> Courses { get; set; }
    public List<SykiUser> Users { get; set; }
    public List<SykiStudent> Students { get; set; }
    public List<SykiTeacher> Teachers { get; set; }
    public List<Discipline> Disciplines { get; set; }
    public List<CourseOffering> CourseOfferings { get; set; }
    public List<AcademicPeriod> AcademicPeriods { get; set; }
    public List<EnrollmentPeriod> EnrollmentPeriods { get; set; }
    public List<CourseCurriculum> CourseCurriculums { get; set; }

    // public List<Classroom> Classrooms { get; set; }
    // public InstitutionSettings Settings { get; set; }
    // public List<WebhookCall> WebhookCalls { get; set; }
    // public List<Notification> Notifications { get; set; }
    // public List<WebhookSubscription> Webhooks { get; set; }

    public Institution() { }

    public Institution(string name)
    {
        Name = name;
        CreatedAt = DateTime.UtcNow;
    }

    public static Institution NewForUserRegister()
    {
        var prefix = new List<string> { "UF", "IF" }.PickRandom();
        var state = Enum.GetValues<BrazilState>().PickRandom();
        return new Institution($"{prefix}{state}");
    }
}
