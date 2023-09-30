using Syki.Dtos;

namespace Syki.Domain;

public class Oferta
{
    public long Id { get; set; }

    public long FaculdadeId { get; set; }

    public long CampusId { get; set; }
    public Campus Campus { get; set; }

    public long CursoId { get; set; }
    public Curso Curso { get; set; }

    public long GradeId { get; set; }
    public Grade Grade { get; set; }

    public string Periodo { get; set; }

    public Turno Turno { get; set; }

    public OfertaOut ToOut()
    {
        return new OfertaOut
        {
            Id = Id,
            Campus = Campus.Nome,
            Curso = Curso.Nome,
            Grade = Grade.Nome,
            Periodo = Periodo,
            Turno = Turno,
        };
    }
}
