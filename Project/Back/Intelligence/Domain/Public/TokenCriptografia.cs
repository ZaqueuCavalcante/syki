namespace Exato.Back.Intelligence.Domain.Public;

public class TokenCriptografia
{
    public int TokenCriptografiaId { get; set; }

    public int ClienteId { get; set; }

    public Guid Token { get; set; }

    public DateTime IncluidoEm { get; set; }

    public int IncluidoPor { get; set; }

    public DateTime? AlteradoEm { get; set; }

    public int? AlteradoPor { get; set; }

    public DateTime? ExcluidoEm { get; set; }

    public int? ExcluidoPor { get; set; }
}
