namespace Exato.Web.Domain;

public class Payment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? CompanyId { get; set; }

    public decimal Value { get; set; }

    public decimal Bonus { get; set; }

    public string? TransactionCode { get; set; }

    public int? CreditPackageId { get; set; }

    public DateTime LastEventDate { get; set; }

    public DateTime? TransactionDate { get; set; }

    public int Status { get; set; }

    public string? Reference { get; set; }

    public bool LastCallEmailSent { get; set; }

    public bool InitialEmailSent { get; set; }

    public DateTime StartDate { get; set; }

    public string? TransactionJson { get; set; }

    public string? Comments { get; set; }

    public int? PaymentProvider { get; set; }

    public string? LastEventJson { get; set; }
}
