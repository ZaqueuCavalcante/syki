using System.ComponentModel;

namespace Syki.Shared;

public enum LessonStatus
{
    [Description("Pendente")]
    Pending,

    [Description("Concluída")]
    Finalized,
}
