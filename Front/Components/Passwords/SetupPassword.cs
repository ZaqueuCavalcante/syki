namespace Syki.Front.Components.Passwords;

public class SetupPassword
{
    public string Password { get; set; }
    public SetupPasswordValidation Validation { get; set; } = new();

    private readonly string _numbers = "0123456789";
    private readonly string _lowers = "abcdefghijklmnopqrstuvwxyz";
    private readonly string _uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private readonly string _nonAlphanumeric = "()~!@#$%^&*-+=|{}[]:;<>,.?/_";

    public void Validate()
    {
        Validation.HasNumbers = Password.IndexOfAny(_numbers.ToCharArray()) >= 0;
        Validation.HasLower = Password.IndexOfAny(_lowers.ToCharArray()) >= 0;
        Validation.HasUpper = Password.IndexOfAny(_uppers.ToCharArray()) >= 0;
        Validation.HasLength = Password.Length >= 8;
        Validation.HasNonAlphanumeric = Password.IndexOfAny(_nonAlphanumeric.ToCharArray()) >= 0;
    }

    public bool IsValid()
    {
        return Validation.IsValid();
    }
}
