namespace Estud.Back.Shared;

/// <summary>
/// Status do Aluno em uma Turma
/// </summary>
public enum StudentClassStatus
{
    [Description("Pendente")]
    Pendente,

    [Description("Matriculado")]
    Matriculado,

    [Description("Aprovado")]
    Aprovado,

    [Description("Dispensado")]
    Dispensado,

    [Description("Reprovado por nota")]
    ReprovadoPorNota,

    [Description("Reprovado por falta")]
    ReprovadoPorFalta,
}
