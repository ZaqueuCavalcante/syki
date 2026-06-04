namespace Syki.Back.Features.Teachers.GetTeacherPotentialCampi;

public class GetTeacherPotentialCampiOut : IApiDto<GetTeacherPotentialCampiOut>
{
    public List<GetTeacherPotentialCampusItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetTeacherPotentialCampiOut)> GetExamples() =>
    [
        ("Exemplo", new() { Items = [new() { Id = 1, Name = "Campus A" }] }),
    ];
}

public class GetTeacherPotentialCampusItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}
