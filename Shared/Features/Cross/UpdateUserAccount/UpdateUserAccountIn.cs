namespace Syki.Shared;

public class UpdateUserAccountIn
{
    /// <summary>
    /// Nome do usuário.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Url da foto de perfil do usuário.
    /// </summary>
    public string? ProfilePhoto { get; set; }
}
