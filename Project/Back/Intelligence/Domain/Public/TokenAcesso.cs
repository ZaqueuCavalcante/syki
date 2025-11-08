namespace Exato.Back.Intelligence.Domain.Public;

public class TokenAcesso
{
    public int TokenAcessoId { get; set; }

    public int ClienteId { get; set; }

    public int? UsuarioId { get; set; }

    public string Token { get; set; }

    public bool AcessoTotal { get; set; }

    public string? IpsAutorizados { get; set; }

    public DateTime? ValidoAte { get; set; }

    public DateTime IncluidoEm { get; set; }

    public int IncluidoPor { get; set; }

    public DateTime? AlteradoEm { get; set; }

    public int? AlteradoPor { get; set; }

    public DateTime? ExcluidoEm { get; set; }

    public int? ExcluidoPor { get; set; }

    public bool? InsertTransaction { get; set; }

    public bool? StoreTransactionInput { get; set; }

    public bool? StoreTransactionReturn { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public Guid? KeyId { get; set; }

    public string? SecretHash { get; set; }

    public short KeyType { get; set; }

    public int? TransLimitPerHour { get; set; }

    public int? TransLimitPerDay { get; set; }

    public int? TransLimitPerWeek { get; set; }

    public int? TransLimitPerMonth { get; set; }

    public int? CreditsLimitPerHour { get; set; }

    public int? CreditsLimitPerDay { get; set; }

    public int? CreditsLimitPerWeek { get; set; }

    public int? CreditsLimitPerMonth { get; set; }

    public decimal? CurrencyLimitPerHour { get; set; }

    public decimal? CurrencyLimitPerDay { get; set; }

    public decimal? CurrencyLimitPerWeek { get; set; }

    public decimal? CurrencyLimitPerMonth { get; set; }

    public int? NfGroup { get; set; }

    /// <summary>
    /// Field to store billing information, created in order to facilitate OMIE integration. <br/>
    /// Created by Marcelo Navarro in 2025-04-29.
    /// </summary>
    public string? BillingInstructions { get; set; }

    public TokenAcesso() { }

    public TokenAcesso(
        int clienteId,
        TokenAcessoKeyType keyType,
        string? name = null,
        string? description = null,
        DateTime? validoAte = null,
        int? transLimitPerHour = null,
        int? transLimitPerDay = null,
        int? transLimitPerWeek = null,
        int? transLimitPerMonth = null,
        int? creditsLimitPerHour = null,
        int? creditsLimitPerDay = null,
        int? creditsLimitPerWeek = null,
        int? creditsLimitPerMonth = null,
        decimal? currencyLimitPerHour = null,
        decimal? currencyLimitPerDay = null,
        decimal? currencyLimitPerWeek = null,
        decimal? currencyLimitPerMonth = null)
    {
        ClienteId = clienteId;
        AcessoTotal = true;
        KeyType = keyType.ToShort();
        Name = name;
        Description = description;

        Token = Guid.NewGuid().ToString("N");

        if (keyType == TokenAcessoKeyType.Type1)
        {
            KeyId = Guid.NewGuid();
            SecretHash = Guid.NewGuid().ToString();
        }

        ValidoAte = validoAte;

        TransLimitPerHour = transLimitPerHour;
        TransLimitPerDay = transLimitPerDay;
        TransLimitPerWeek = transLimitPerWeek;
        TransLimitPerMonth = transLimitPerMonth;

        CreditsLimitPerHour = creditsLimitPerHour;
        CreditsLimitPerDay = creditsLimitPerDay;
        CreditsLimitPerWeek = creditsLimitPerWeek;
        CreditsLimitPerMonth = creditsLimitPerMonth;

        CurrencyLimitPerHour = currencyLimitPerHour;
        CurrencyLimitPerDay = currencyLimitPerDay;
        CurrencyLimitPerWeek = currencyLimitPerWeek;
        CurrencyLimitPerMonth = currencyLimitPerMonth;

        IncluidoPor = 7;
        IncluidoEm = DateTime.Now;
    }

    public TokenAcesso(
        int clienteId,
        int usuarioId)
    {
        ClienteId = clienteId;
        UsuarioId = usuarioId;

        AcessoTotal = true;
        Token = Guid.NewGuid().ToString("N");
        KeyType = TokenAcessoKeyType.Type3.ToShort();

        IncluidoPor = 7;
        IncluidoEm = DateTime.Now;
    }

    public void Editar(
        string? name = null,
        string? description = null,
        DateTime? validoAte = null,
        int? transLimitPerHour = null,
        int? transLimitPerDay = null,
        int? transLimitPerWeek = null,
        int? transLimitPerMonth = null,
        int? creditsLimitPerHour = null,
        int? creditsLimitPerDay = null,
        int? creditsLimitPerWeek = null,
        int? creditsLimitPerMonth = null,
        decimal? currencyLimitPerHour = null,
        decimal? currencyLimitPerDay = null,
        decimal? currencyLimitPerWeek = null,
        decimal? currencyLimitPerMonth = null)
    {
        Name = name;
        Description = description;

        ValidoAte = validoAte;

        TransLimitPerHour = transLimitPerHour;
        TransLimitPerDay = transLimitPerDay;
        TransLimitPerWeek = transLimitPerWeek;
        TransLimitPerMonth = transLimitPerMonth;

        CreditsLimitPerHour = creditsLimitPerHour;
        CreditsLimitPerDay = creditsLimitPerDay;
        CreditsLimitPerWeek = creditsLimitPerWeek;
        CreditsLimitPerMonth = creditsLimitPerMonth;

        CurrencyLimitPerHour = currencyLimitPerHour;
        CurrencyLimitPerDay = currencyLimitPerDay;
        CurrencyLimitPerWeek = currencyLimitPerWeek;
        CurrencyLimitPerMonth = currencyLimitPerMonth;

        AlteradoEm = DateTime.Now;
    }

    public void SoftDelete()
    {
        ExcluidoEm = DateTime.Now;
        ExcluidoPor = 7;
    }
}
