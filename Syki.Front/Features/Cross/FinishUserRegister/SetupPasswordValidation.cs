namespace Syki.Front.FinishUserRegister;

public class SetupPasswordValidation
{
    public bool HasNumbers { get; set; }
    public bool HasLower { get; set; }
    public bool HasUpper { get; set; }
    public bool HasLength { get; set; }
    public bool HasNonAlphanumeric { get; set; }

    public bool IsValid()
    {
        return HasNumbers && HasLower && HasUpper && HasLength && HasNonAlphanumeric;
    }
}
