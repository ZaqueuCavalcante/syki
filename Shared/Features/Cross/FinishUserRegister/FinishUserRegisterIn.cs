namespace Syki.Shared;

public class FinishUserRegisterIn
{
    /// <summary>
    /// Token enviado para o email informado no cadastro.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Senha definida pelo usuário, seguindo os requisitos de segurança exigidos.
    /// </summary>
    public string Password { get; set; }

    public FinishUserRegisterIn(string? token, string password)
    {
        Token = token;
        Password = password;
    }
}
