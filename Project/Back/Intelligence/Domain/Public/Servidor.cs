namespace Exato.Back.Intelligence.Domain.Public;

public class Servidor
{
    public short ServidorId { get; set; }

    public string NomeMaquina { get; set; }

    public string IpInternoEntrada { get; set; }

    public bool ProcessarLotes { get; set; }
}
