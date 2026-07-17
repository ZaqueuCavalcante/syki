namespace Estud.Back.Features.Classes.CreateClass;

public class CreateClassIn : IApiDto<CreateClassIn>
{
    public int DisciplineId { get; set; }
    public int PeriodId { get; set; }
    public int Vacancies { get; set; }
    public int? CampusId { get; set; }

    public static IEnumerable<(string, CreateClassIn)> GetExamples() =>
    [
        ("Banco de Dados",
        new CreateClassIn
        {
            DisciplineId = 1,
            PeriodId = 2,
            Vacancies = 40,
            CampusId = 1,
        }),
    ];
}
