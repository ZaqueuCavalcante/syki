namespace Exato.Back.Intelligence.Domain.Ibge;

public class CnaeConsolidado
{
    public int Id { get; set; }

    public string Secao { get; set; }

    public string? Divisao { get; set; }

    public short? DivisaoNum { get; set; }

    public string? Grupo { get; set; }

    public short? GrupoNum { get; set; }

    public string? Classe { get; set; }

    public int? ClasseNum { get; set; }

    public string? Subclasse { get; set; }

    public int? SubclasseNum { get; set; }

    public string Denominacao { get; set; }

    public decimal Versao { get; set; }

    public short ControleId { get; set; }

    public string Tipo { get; set; }

    public int? SegmentoQuod { get; set; }
}
