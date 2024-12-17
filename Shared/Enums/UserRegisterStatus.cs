using System.ComponentModel;

namespace Syki.Shared;

public enum UserRegisterStatus
{
    [Description("Pendente")]
    Pending,

    [Description("Concluído")]
    Finished,
}
