namespace Exato.Web.Domain;

public class UserPhoneNumber
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? CompanyId { get; set; }

    public string Ddd { get; set; }

    public string Number { get; set; }

    public bool Main { get; set; }

    public bool Verified { get; set; }

    public DateTime? VerificationDate { get; set; }

    public int Type { get; set; }
}
