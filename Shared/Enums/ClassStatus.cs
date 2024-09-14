using System.ComponentModel;

namespace Syki.Shared;

public enum ClassStatus
{
    [Description("Em período de matrícula")]
    OnEnrollmentPeriod,

    [Description("Pré-matrícula")]
    OnPreEnrollment,

    [Description("Matrícula")]
    OnEnrollment,

    [Description("Pendente")]
    Pending,

    [Description("Iniciada")]
    Started,

    [Description("Finalizada")]
    Finalized,
}
