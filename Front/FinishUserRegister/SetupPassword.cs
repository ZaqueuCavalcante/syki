namespace Syki.Front.FinishUserRegister;

public class SetupPassword
{
    public string Password { get; set; }

    public SetupPasswordValidation Validation { get; set; } = new();

    private readonly string Numbers = "0123456789";
    private readonly string Lowers = "abcdefghijklmnopqrstuvwxyz";
    private readonly string Uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private readonly string NonAlphanumeric = "()~!@#$%^&*-+=|{}[]:;<>,.?/_";

    public void Validate()
    {
        Validation.HasNumbers = Password.IndexOfAny(Numbers.ToCharArray()) >= 0;
        Validation.HasLower = Password.IndexOfAny(Lowers.ToCharArray()) >= 0;
        Validation.HasUpper = Password.IndexOfAny(Uppers.ToCharArray()) >= 0;
        Validation.HasLength = Password.Length >= 8;
        Validation.HasNonAlphanumeric = Password.IndexOfAny(NonAlphanumeric.ToCharArray()) >= 0;
    }

    public bool IsValid()
    {
        return Validation.IsValid();
    }
}
