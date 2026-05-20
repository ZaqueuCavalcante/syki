using Syki.Back.Domain.Disciplines;

namespace Syki.Back.Domain.Courses;

/// <summary>
/// Grade Curricular de um Curso.
/// Um mesmo Curso pode possuir diversas Grades diferentes.
/// </summary>
public class CourseCurriculum
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public int CourseId { get; set; }
    public string Name { get; set; }
    public List<Discipline> Disciplines { get; set; }
    public List<CourseCurriculumDiscipline> Links { get; set; }

    public Course? Course { get; set; }

    public CourseCurriculum(
        int institutionId,
        int courseId,
        string name
    ) {
        InstitutionId = institutionId;
        CourseId = courseId;
        Name = name;
        Disciplines = [];
        Links = [];
    }

    public void AddDisciplines(List<CourseCurriculumDiscipline> disciplines)
    {
        disciplines.ForEach(d => Links.Add(d));
    }
}
