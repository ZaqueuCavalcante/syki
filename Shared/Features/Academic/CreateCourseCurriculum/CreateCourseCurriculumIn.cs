namespace Syki.Shared;

public class CreateCourseCurriculumIn
{
    public string Name { get; set; }
    public Guid CourseId { get; set; }
    public List<CreateCourseCurriculumDisciplineIn> Disciplines { get; set; } = [];
}
