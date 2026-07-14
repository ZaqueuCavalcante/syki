namespace Estud.Back.Features.Institutions.GetInstitutionConfig;

public class GetInstitutionConfigOut : IApiDto<GetInstitutionConfigOut>
{
    public int Id { get; set; }

    /// <summary>
    /// Nota mínima para aprovação na disciplina (de 0 a 10)
    /// </summary>
    public decimal NoteLimit { get; set; }

    /// <summary>
    /// Frequência mínima para aprovação na disciplina (de 0% a 100%)
    /// </summary>
    public decimal FrequencyLimit { get; set; }

    public static IEnumerable<(string, GetInstitutionConfigOut)> GetExamples() =>
    [
        ("Padrão",
        new GetInstitutionConfigOut
        {
            Id = 1,
            NoteLimit = 7.00M,
            FrequencyLimit = 70.00M,
        }),
    ];
}
