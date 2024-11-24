namespace Syki.Shared;

public class CreatePendingUserRegisterIn
{
    /// <summary>
    /// Um link de finalização de cadastro será enviado para esse email.
    /// </summary>
    public string Email { get; set; }

    public CreatePendingUserRegisterIn(string email)
    {
        Email = email;
    }
}
