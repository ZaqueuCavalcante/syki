namespace Syki.Dtos;

public class OfertaIn
{
    public long CampusId { get; set; }
    public long CursoId { get; set; }
    public long GradeId { get; set; }
    public string Periodo { get; set; }
    public Turno Turno { get; set; }
}
