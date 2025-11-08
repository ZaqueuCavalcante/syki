namespace Exato.Back.Intelligence.Domain.Public;

public class UserPhoneNumber
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? AreaCode { get; set; }

    public string Number { get; set; }

    public short PhoneType { get; set; }

    public int? CountryCode { get; set; }

    public string? OriginalInput { get; set; }

    public int? ClienteId { get; set; }

    public DateTime? MigratedAt { get; set; }

    public int? MigratedFromClienteId { get; set; }

    public int? MigratedFromUserId { get; set; }

    public bool? Verified { get; set; }

    public DateTime? VerificationDate { get; set; }
}
