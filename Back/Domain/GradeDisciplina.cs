namespace Syki.Domain;

public class GradeDisciplina
{
    public long GradeId { get; set; }
    
    public long DisciplinaId { get; set; }

    public byte Periodo { get; set; }

    public byte Creditos { get; set; }

    public ushort CargaHoraria { get; set; }
}
