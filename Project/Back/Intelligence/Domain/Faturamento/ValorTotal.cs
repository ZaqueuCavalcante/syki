namespace Exato.Back.Intelligence.Domain.Faturamento;

public class ValorTotal
{
    public int Id { get; set; }

    public int ParentOrganizationId { get; set; }

    public decimal ValorTotalCreditos { get; set; }

    public decimal ValorTotalRelatorios { get; set; }

    public DateTime InseridoEm { get; set; }

    public DateTime? AlteradoEm { get; set; }

    public int? AnoMes { get; set; }

    public decimal? ValorTotalDoccheck { get; set; }

    public decimal? FranquiaMinima { get; set; }

    public decimal? ValorConsumo { get; set; }

    public decimal? ValorFinal { get; set; }

    public int? NfGroup { get; set; }

    public long? ContractCode { get; set; }
}
