namespace Estud.Back.Features.Institutions.SetupInstitutionConfig;

public class SetupInstitutionConfigIn : IApiDto<SetupInstitutionConfigIn>
{
    /// <summary>
    /// Nota mínima para aprovação na disciplina (de 0 a 10)
    /// </summary>
    public decimal NoteLimit { get; set; }

    /// <summary>
    /// Frequência mínima para aprovação na disciplina (de 0% a 100%)
    /// </summary>
    public decimal FrequencyLimit { get; set; }

    public static IEnumerable<(string, SetupInstitutionConfigIn)> GetExamples() =>
    [
        ("Padrão",
        new SetupInstitutionConfigIn
        {
            NoteLimit = 7.00M,
            FrequencyLimit = 70.00M,
        }),

        ("Mais rigorosa",
        new SetupInstitutionConfigIn
        {
            NoteLimit = 8.50M,
            FrequencyLimit = 85.00M,
        }),
    ];
}
