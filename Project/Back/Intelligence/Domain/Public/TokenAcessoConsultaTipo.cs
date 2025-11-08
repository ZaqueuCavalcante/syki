namespace Exato.Back.Intelligence.Domain.Public;

public class TokenAcessoConsultaTipo
{
    public int TokenAcessoConsultaTipoId { get; set; }

    public int? TokenAcessoId { get; set; }

    public short? ConsultaTipoId { get; set; }

    public DateTime IncluidoEm { get; set; }

    public int IncluidoPor { get; set; }
}
