namespace Exato.Back.Intelligence.Domain.Public;

public class TokenAcessoQuantidade
{
    public int TokenAcessoId { get; set; }

    public DateOnly Day { get; set; }

    public TimeOnly Hour { get; set; }

    public int TransTotal { get; set; }

    public int CreditsTotal { get; set; }

    public decimal CurrencyTotal { get; set; }
}
