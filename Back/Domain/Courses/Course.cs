using Syki.Back.Domain.Disciplines;
using Syki.Back.Domain.CourseCurriculums;

namespace Syki.Back.Domain.Courses;

/// <summary>
/// Curso
/// </summary>
public class Course
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public string Name { get; set; }
    public CourseType CourseType { get; set; }
    public List<Discipline> Disciplines { get; set; }
    public List<CourseDiscipline> Links { get; set; }
    public List<CourseCurriculum> CourseCurriculums { get; set; }

    private Course() {}

    public Course(
        int institutionId,
        string name,
        CourseType courseType
    ) {
        InstitutionId = institutionId;
        Name = name;
        CourseType = courseType;
        Links = [];
        Disciplines = [];
        CourseCurriculums = [];
    }

    public void Update(string name, CourseType courseType)
    {
        Name = name;
        CourseType = courseType;
    }
}
