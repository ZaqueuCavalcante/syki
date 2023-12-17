namespace Syki.Shared;

public class GradeDisciplinaIn
{
    public Guid Id { get; set; }
    public byte Periodo { get; set; }
    public byte Creditos { get; set; }
    public ushort CargaHoraria { get; set; }

    public GradeDisciplinaIn() { }

    public GradeDisciplinaIn(
        Guid disciplinaId,
        byte periodo,
        byte creditos,
        ushort cargaHoraria
    ) {
        Id = disciplinaId;
        Periodo = periodo;
        Creditos = creditos;
        CargaHoraria = cargaHoraria;
    }
}
