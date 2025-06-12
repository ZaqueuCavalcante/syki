namespace Syki.Shared;

public class GetUserAccountOut
{
    /// <summary>
    /// Nome do usuário.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Email do usuário.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Instituição do usuário.
    /// </summary>
    public string Institution { get; set; }

    /// <summary>
    /// Role do usuário.
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Url da foto de perfil do usuário.
    /// </summary>
    public string ProfilePhoto { get; set; }

    /// <summary>
    /// Curso, caso seja um Aluno.
    /// </summary>
    public string? Course { get; set; }
}
