namespace Exato.Shared.Extensions;

public static class GuidExtensions
{
    public static int ToHashCode(this Guid guid)
    {
        var justNumbers = guid.ToString().OnlyNumbers();
        return int.Parse(justNumbers[^8..]);
    }
}
