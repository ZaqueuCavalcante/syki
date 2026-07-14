namespace Estud.Back.Features.Institutions.SetupInstitutionConfig;

public class SetupInstitutionConfigOut : IApiDto<SetupInstitutionConfigOut>
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

    public static IEnumerable<(string, SetupInstitutionConfigOut)> GetExamples() =>
    [
        ("Padrão",
        new SetupInstitutionConfigOut
        {
            Id = 1,
            NoteLimit = 7.00M,
            FrequencyLimit = 70.00M,
        }),

        ("Mais rigorosa",
        new SetupInstitutionConfigOut
        {
            Id = 1,
            NoteLimit = 8.50M,
            FrequencyLimit = 85.00M,
        }),
    ];
}
