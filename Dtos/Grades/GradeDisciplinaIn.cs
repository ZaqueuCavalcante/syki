namespace Syki.Dtos;

public class GradeDisciplinaIn
{
    public Guid Id { get; set; }
    public byte Periodo { get; set; }
    public byte Creditos { get; set; }
    public ushort CargaHoraria { get; set; }
}
