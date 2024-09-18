using System.ComponentModel;

namespace Syki.Shared;

public enum ClassStatus
{
    /// <summary>
    /// Ao criar uma Turma, ela inicia com esse status.
    /// Um Turma só pode chegar no próximo status caso o request seja feito dentro do Período de Matrícula vigente.
    /// </summary>
    [Description("Pré-matrícula")]
    OnPreEnrollment = 0,

    /// <summary>
    /// Caso o Período de Matrícula termine, a Turma vai continuar com esse status até que seja iniciada.
    /// O Período de Matrícula pode ser prorrogado também, de forma que o status permanece o mesmo.
    /// </summary>
    [Description("Matrícula")]
    OnEnrollment = 1,

    /// <summary>
    /// Uma Turma só pode ser iniciada ao final do Período de Matrícula.
    /// Após iniciada, não é possível retroceder para algum status anterior a esse.
    /// </summary>
    [Description("Iniciada")]
    Started = 2,

    /// <summary>
    /// Ao final do semestre, quando todas as notas e frequências forem salvas, é possível finalizar a Turma.
    /// </summary>
    [Description("Finalizada")]
    Finalized = 3,
}
