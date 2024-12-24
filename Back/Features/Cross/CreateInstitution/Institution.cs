using Syki.Back.Features.Academic.CreateCourse;
using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateDiscipline;
using Syki.Back.Features.Academic.CreateNotification;
using Syki.Back.Features.Academic.CreateCourseOffering;
using Syki.Back.Features.Academic.CreateAcademicPeriod;
using Syki.Back.Features.Academic.CreateCourseCurriculum;

namespace Syki.Back.Features.Cross.CreateInstitution;

public class Institution : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Campus> Campi { get; set; }
    public List<Course> Courses { get; set; }
    public List<SykiStudent> Students { get; set; }
    public InstitutionConfigs Configs { get; set; }
    public List<SykiTeacher> Teachers { get; set; }
    public List<Discipline> Disciplines { get; set; }
    public List<Notification> Notifications { get; set; }
    public List<CourseOffering> CourseOfferings { get; set; }
    public List<AcademicPeriod> AcademicPeriods { get; set; }
    public List<CourseCurriculum> CourseCurriculums { get; set; }

    public Institution() { }

    public Institution(Guid id, string name)
    {
        Id = id;
        Name = name;
        CreatedAt = DateTime.Now;
        Configs = new InstitutionConfigs(Id, 7, 70);

        AddDomainEvent(new InstitutionCreatedDomainEvent(Id));
    }

    public InstitutionOut ToOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            CreatedAt = CreatedAt,
            NoteLimit = Configs.NoteLimit,
            FrequencyLimit = Configs.FrequencyLimit,
        };
    }
}
