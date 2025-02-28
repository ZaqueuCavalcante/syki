using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Status do Aluno em uma Disciplina
/// </summary>
public enum StudentDisciplineStatus
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
