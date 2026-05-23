namespace Syki.Back.Features.Disciplines.UpdateDiscipline;

public class UpdateDisciplineIn : IApiDto<UpdateDisciplineIn>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static IEnumerable<(string, UpdateDisciplineIn)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1, Name = "Cálculo I" }),
    ];
}
