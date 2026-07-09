namespace Estud.Back.Features.Disciplines.UpdateDiscipline;

public class UpdateDisciplineOut : IApiDto<UpdateDisciplineOut>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static IEnumerable<(string, UpdateDisciplineOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1, Name = "Cálculo I" }),
    ];
}
