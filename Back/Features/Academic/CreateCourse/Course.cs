using Syki.Back.Features.Academic.CreateDiscipline;
using Syki.Back.Features.Academic.CreateCourseCurriculum;

namespace Syki.Back.Features.Academic.CreateCourse;

/// <summary>
/// Curso
/// </summary>
public class Course
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public CourseType Type { get; set; }
    public List<Discipline> Disciplines { get; set; }
    public List<CourseDiscipline> Links { get; set; }
    public List<CourseCurriculum> CourseCurriculums { get; set; }

    private Course() {}

    public Course(
        Guid institutionId,
        string name,
        CourseType type
    ) {
        Id = Guid.CreateVersion7();
        InstitutionId = institutionId;
        Name = name;
        Type = type;
        Links = [];
        Disciplines = [];
    }

    public CourseOut ToOut()
    {
        return new CourseOut
        {
            Id = Id,
            Name = Name,
            Type = Type,
            Disciplines = Disciplines?.ConvertAll(x => x.ToOut()) ?? [],
        };
    }
}
