using Syki.Shared;

namespace Syki.Back.Domain;

public class Oferta
{
    public Guid Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public Guid CampusId { get; set; }
    public Campus Campus { get; set; }
    public Guid CursoId { get; set; }
    public Curso Curso { get; set; }
    public Guid GradeId { get; set; }
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
            GradeId = Grade.Id,
            Grade = Grade.Nome,
            Periodo = Periodo,
            Turno = Turno,
        };
    }
}
