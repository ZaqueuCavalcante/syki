namespace Syki.Dtos;

public class OfertaIn
{
    public Guid CampusId { get; set; }
    public Guid CursoId { get; set; }
    public Guid GradeId { get; set; }
    public string Periodo { get; set; }
    public Turno Turno { get; set; }
}
