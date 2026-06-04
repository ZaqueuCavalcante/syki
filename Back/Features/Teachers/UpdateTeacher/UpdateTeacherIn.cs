namespace Syki.Back.Features.Teachers.UpdateTeacher;

public class UpdateTeacherIn : IApiDto<UpdateTeacherIn>
{
    public string Name { get; set; }
    public string Email { get; set; }

    public static IEnumerable<(string, UpdateTeacherIn)> GetExamples() =>
    [
        ("Exemplo", new() { Name = "Richard Feynman", Email = "feynman@syki.com" }),
    ];
}
