namespace Exato.Back.Intelligence.Domain.Public;

/// <summary>
/// 1 = PF <br/>
/// 2 = PJ <br/>
/// 3 = PF + Quod <br/>
/// 4 = PJ + Quod <br/>
/// </summary>
public class ConsultaRelatorioTipo
{
    public int Id { get; set; }

    public string Tipo { get; set; }
}
