using System.ComponentModel;

namespace Syki.Shared;

public enum StudentStatus
{
    [Description("Matriculado")]
    Enrolled,

    [Description("Transferido")]
    Transferred,

    [Description("Trancado")]
    Deferred,

    [Description("Concluído")]
    Completed,
}
