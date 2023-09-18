using Syki.Dtos;

namespace Syki.Domain;

public class CursoOfertado
{
    public long Id { get; set; }

    public long FaculdadeId { get; set; }

    public long CampusId { get; set; }

    public long CursoId { get; set; }

    public long GradeId { get; set; }

    public string PeriodoId { get; set; }

    public Turno Turno { get; set; }
}
