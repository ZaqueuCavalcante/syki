using System.ComponentModel;

namespace Syki.Back.Shared;

/// <summary>
/// Status do registro de um Usuário
/// </summary>
public enum UserRegisterStatus
{
    [Description("Pendente")]
    Pending,

    [Description("Concluído")]
    Finished,
}
