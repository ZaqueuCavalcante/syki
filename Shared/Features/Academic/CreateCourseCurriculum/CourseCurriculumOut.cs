namespace Syki.Shared;

public class CourseCurriculumOut
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string CourseName { get; set; }
    public string Name { get; set; }
    public List<DisciplineOut> Disciplines { get; set; }

    public static implicit operator CourseCurriculumOut(OneOf<CourseCurriculumOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
