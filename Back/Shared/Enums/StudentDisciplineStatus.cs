namespace Estud.Back.Shared;

/// <summary>
/// Status de uma Disciplina da grade em relação ao Aluno
/// </summary>
public enum StudentDisciplineStatus
{
    /// <summary>
    /// O aluno ainda não cursou a disciplina (vai cursar).
    /// </summary>
    [Description("Não cursada")]
    NaoCursada,

    /// <summary>
    /// O aluno está cursando a disciplina atualmente.
    /// </summary>
    [Description("Cursando")]
    Cursando,

    /// <summary>
    /// O aluno já cursou e foi aprovado na disciplina.
    /// </summary>
    [Description("Aprovada")]
    Aprovada,

    /// <summary>
    /// O aluno foi dispensado da disciplina.
    /// </summary>
    [Description("Dispensada")]
    Dispensada,

    /// <summary>
    /// O aluno já cursou e foi reprovado na disciplina (por nota ou por falta).
    /// </summary>
    [Description("Reprovada")]
    Reprovada,
}
