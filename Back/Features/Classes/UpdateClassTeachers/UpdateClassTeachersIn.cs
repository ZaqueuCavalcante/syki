namespace Estud.Back.Features.Classes.UpdateClassTeachers;

public class UpdateClassTeachersIn : IApiDto<UpdateClassTeachersIn>
{
    public List<int> Teachers { get; set; } = [];

    public static IEnumerable<(string, UpdateClassTeachersIn)> GetExamples() =>
    [
        ("Exemplo", new() { Teachers = [14, 32] }),
    ];
}
