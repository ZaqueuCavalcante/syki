using Syki.Shared;
using Syki.Back.CreateCampus;

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

    public Oferta(
        Guid faculdadeId,
        Guid campusId,
        Guid cursoId,
        Guid gradeId,
        string periodo,
        Turno turno
    ) {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        CampusId = campusId;
        CursoId = cursoId;
        GradeId = gradeId;
        Periodo = periodo;
        Turno = turno;
    }

    public OfertaOut ToOut()
    {
        return new OfertaOut
        {
            Id = Id,
            Campus = Campus.Name,
            Curso = Curso.Nome,
            GradeId = GradeId,
            Grade = Grade.Nome,
            Periodo = Periodo,
            Turno = Turno,
        };
    }
}
