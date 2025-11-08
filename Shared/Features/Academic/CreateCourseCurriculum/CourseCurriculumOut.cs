namespace Syki.Shared;

public class CourseCurriculumOut : IApiDto<CourseCurriculumOut>
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string CourseName { get; set; }
    public string Name { get; set; }
    public List<DisciplineOut> Disciplines { get; set; }

    public static IEnumerable<(string, CourseCurriculumOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = Guid.CreateVersion7(), CourseId = Guid.CreateVersion7(), CourseName = "ADS", Name = "Grade ADS", Disciplines = [] }),
    ];

    public static implicit operator CourseCurriculumOut(OneOf<CourseCurriculumOut, ErrorOut> value)
    {
        return value.Success;
    }
}
