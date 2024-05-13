using Syki.Back.Features.Academic.CreateCourse;
using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

public class CourseCurriculum
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public string Name { get; set; }
    public List<Discipline> Disciplines { get; set; }
    public List<CourseCurriculumDiscipline> Links { get; set; }

    public CourseCurriculum(
        Guid institutionId,
        Guid courseId,
        string name
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        CourseId = courseId;
        Name = name;
        Disciplines = [];
        Links = [];
    }

    public CourseCurriculumOut ToOut()
    {
        var result = new CourseCurriculumOut
        {
            Id = Id,
            CourseId = CourseId,
            CourseName = Course.Name,
            Name = Name,
            Disciplines = Disciplines.ConvertAll(d => d.ToOut()),
        };

        foreach (var link in Links)
        {
            var discipline = result.Disciplines.First(x => x.Id == link.DisciplineId);
            discipline.Period = link.Period;
            discipline.Credits = link.Credits;
            discipline.Workload = link.Workload;
        }

        return result;
    }
}
