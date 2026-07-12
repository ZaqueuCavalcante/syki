using Estud.Back.Domain.Campi;
using Estud.Back.Domain.Classes;
using Estud.Back.Domain.Courses;
using Estud.Back.Domain.Periods;
using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Students;
using Estud.Back.Domain.Teachers;
using Estud.Back.Domain.Webhooks;
using Estud.Back.Domain.Commands;
using Estud.Back.Domain.Calendar;
using Estud.Back.Domain.Classrooms;
using Estud.Back.Domain.Disciplines;
using Estud.Back.Domain.Notifications;
using Estud.Back.Domain.CourseOfferings;
using Estud.Back.Domain.CourseCurriculums;

namespace Estud.Back.Domain.Institutions;

public class Institution
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Campus> Campi { get; set; }
    public List<Class> Classes { get; set; }
    public List<Course> Courses { get; set; }
    public List<EstudUser> Users { get; set; }
    public List<Command> Commands { get; set; }
    public List<Classroom> Classrooms { get; set; }
    public List<EstudStudent> Students { get; set; }
    public List<EstudTeacher> Teachers { get; set; }
    public List<Discipline> Disciplines { get; set; }
    public List<CalendarDay> CalendarDays { get; set; }
    public List<WebhookCall> WebhookCalls { get; set; }
    public List<Notification> Notifications { get; set; }
    public List<CommandBatch> CommandBatches { get; set; }
    public List<CourseOffering> CourseOfferings { get; set; }
    public List<AcademicPeriod> AcademicPeriods { get; set; }
    public List<SsoConfiguration> SsoConfigurations { get; set; }
    public List<EnrollmentPeriod> EnrollmentPeriods { get; set; }
    public List<CourseCurriculum> CourseCurriculums { get; set; }
    public List<ResetPasswordToken> ResetPasswordTokens { get; set; }
    public List<WebhookSubscription> WebhookSubscriptions { get; set; }

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
