namespace Exato.Back.Intelligence.Domain.Public;

public class ConsultaResultadoTipo
{
    public short ConsultaResultadoTipoId { get; set; }

    public string Nome { get; set; }

    public bool Definitivo { get; set; }

    public bool Faturavel { get; set; }

    public bool Erro { get; set; }

    public bool GeraComprovante { get; set; }

    public bool GeraRegistroConsulta { get; set; }
}
