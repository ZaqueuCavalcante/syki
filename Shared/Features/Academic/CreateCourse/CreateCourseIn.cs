namespace Syki.Shared;

public class CreateCourseIn
{
    /// <summary>
    /// Nome do curso
    /// </summary>
    public string Name { get; set; }

    public CourseType Type { get; set; }

    /// <summary>
    /// Nome das disciplinas que serão criadas junto com o curso
    /// </summary>
    public List<string> Disciplines { get; set; } = [];
}
