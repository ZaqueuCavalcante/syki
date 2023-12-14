namespace Syki.Back.Domain;

public class GradeDisciplina
{
    public Guid GradeId { get; set; }
    public Guid DisciplinaId { get; set; }
    public byte Periodo { get; set; }
    public byte Creditos { get; set; }
    public ushort CargaHoraria { get; set; }
}
