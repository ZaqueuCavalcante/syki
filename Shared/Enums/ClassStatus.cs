using System.ComponentModel;

namespace Syki.Shared;

public enum ClassStatus
{
    [Description("Em período de matrícula")]
    OnEnrollmentPeriod,

    [Description("Aguardando revisão")]
    WaitingReview,

    [Description("Iniciada")]
    Started,

    [Description("Finalizada")]
    Finalized,
}
