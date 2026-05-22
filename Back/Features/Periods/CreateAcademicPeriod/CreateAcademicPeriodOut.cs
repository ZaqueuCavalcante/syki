namespace Syki.Back.Features.Periods.CreateAcademicPeriod;

public class CreateAcademicPeriodOut : IApiDto<CreateAcademicPeriodOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateAcademicPeriodOut)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
