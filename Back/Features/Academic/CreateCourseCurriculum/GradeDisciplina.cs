namespace Syki.Back.Features.Academic.CreateGrade;

public class GradeDisciplina
{
    public Guid GradeId { get; set; }
    public Guid DisciplinaId { get; set; }
    public byte Periodo { get; set; }
    public byte Creditos { get; set; }
    public ushort CargaHoraria { get; set; }

    public GradeDisciplina(
        Guid disciplinaId,
        byte periodo,
        byte creditos,
        ushort cargaHoraria
    ) {
        DisciplinaId = disciplinaId;
        Periodo = periodo;
        Creditos = creditos;
        CargaHoraria = cargaHoraria;
    }
}
