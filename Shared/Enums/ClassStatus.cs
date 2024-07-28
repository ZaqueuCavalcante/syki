using System.ComponentModel;

namespace Syki.Shared;

public enum ClassStatus
{
    [Description("Em período de matrícula")]
    OnEnrollmentPeriod,

    [Description("Iniciada")]
    Started,

    [Description("Finalizada")]
    Finalized,
}
