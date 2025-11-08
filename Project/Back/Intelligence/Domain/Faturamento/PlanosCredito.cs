namespace Exato.Back.Intelligence.Domain.Faturamento;

public class PlanosCredito
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public decimal? Zero2k { get; set; }

    public decimal? Dois5k { get; set; }

    public decimal? Cinco10k { get; set; }

    public decimal? Dez25k { get; set; }

    public decimal? VinteCinco50k { get; set; }

    public decimal? Cinquenta100k { get; set; }

    public decimal? Cem250k { get; set; }

    public decimal? DuzentosCinquenta500k { get; set; }

    public decimal? Quinhentos1m { get; set; }

    public decimal? Acima1m { get; set; }

    public bool? IsDefault { get; set; }
}
