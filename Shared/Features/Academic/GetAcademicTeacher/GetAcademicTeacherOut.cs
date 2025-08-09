namespace Syki.Shared;

public class GetAcademicTeacherOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<GetAcademicTeacherDisciplineOut> Disciplines { get; set; } = [];
    public List<GetAcademicTeacherCampusOut> Campi { get; set; } = [];

    public static IEnumerable<(string, GetAcademicTeacherOut)> GetExamples() =>
    [
        ("Exemplo", new() {
            Id = Guid.CreateVersion7(),
            Name = "Davi Pessoa da Silva",
            Email = "davi.pessoa@syki.com.br",
            Disciplines = [
                new() { Id = Guid.CreateVersion7(), Name = "Banco de Dados" },
                new() { Id = Guid.CreateVersion7(), Name = "Programação Orientada a Objetos" },
            ],
            Campi = [
                new() { Id = Guid.CreateVersion7(), Name = "Suassuna" },
                new() { Id = Guid.CreateVersion7(), Name = "Agreste" },
            ]
        }),
    ];
}
