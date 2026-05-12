using Syki.Back.Features.Academic.CreateCourse;
using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

/// <summary>
/// Grade Curricular de um Curso.
/// Um mesmo Curso pode possuir diversas Grades diferentes.
/// </summary>
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
        Id = Guid.CreateVersion7();
        InstitutionId = institutionId;
        CourseId = courseId;
        Name = name;
        Disciplines = [];
        Links = [];
    }

    public void AddDisciplines(List<CreateCourseCurriculumDisciplineIn> disciplines)
    {
        // TODO: Validar que não têm disciplinas repetidas (mesmo id)

        disciplines.ForEach(d =>
            Links.Add(new(d.Id, d.Period, d.Credits, d.Workload))
        );
    }

    public OneOf<SykiSuccess, SykiError> AddDisciplinePreRequisites(Guid targetDisciplineId, List<Guid> preRequisites)
    {
        if (!Links.Any(x => x.DisciplineId == targetDisciplineId))
            return new DisciplineNotFound();

        if (preRequisites.Contains(targetDisciplineId) || !preRequisites.IsSubsetOf(Links.ConvertAll(d => d.DisciplineId)))
            return new InvalidDisciplinesList();

        var targetDiscipline = Links.First(x => x.DisciplineId == targetDisciplineId);
        foreach (var id in preRequisites)
        {
            if (Links.First(x => x.DisciplineId == id).Period >= targetDiscipline.Period)
                return new InvalidDisciplinesList();
        }

        targetDiscipline.AddPreRequisites(preRequisites);

        return new SykiSuccess();
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
            discipline.PreRequisites = link.PreRequisites;
        }

        return result;
    }
}
