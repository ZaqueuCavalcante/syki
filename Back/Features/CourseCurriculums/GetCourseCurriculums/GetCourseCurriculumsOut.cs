namespace Estud.Back.Features.CourseCurriculums.GetCourseCurriculums;

public class GetCourseCurriculumsOut : IApiDto<GetCourseCurriculumsOut>
{
    public int Total { get; set; }
    public List<GetCourseCurriculumsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetCourseCurriculumsOut)> GetExamples() =>
    [
        ("Exemplo", new() { Total = 1, Items = [new() { Id = 1, Name = "Grade ADS 2024", CourseId = 1, Course = "Análise e Desenvolvimento de Sistemas" }] }),
    ];
}

public class GetCourseCurriculumsItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CourseId { get; set; }
    public string Course { get; set; }
    public int Disciplines { get; set; }
}
