using Syki.Back.Features.Academico.CreateCurso;
using Syki.Back.Features.Academico.CreateCampus;
using Syki.Back.Features.Academico.CreateGrade;

namespace Syki.Back.CreateOferta;

public class Oferta
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid CampusId { get; set; }
    public Campus Campus { get; set; }
    public Guid CursoId { get; set; }
    public Curso Curso { get; set; }
    public Guid GradeId { get; set; }
    public Grade Grade { get; set; }
    public string Periodo { get; set; }
    public Turno Turno { get; set; }

    public Oferta(
        Guid institutionId,
        Guid campusId,
        Guid cursoId,
        Guid gradeId,
        string periodo,
        Turno turno
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
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
            Curso = Curso.Name,
            GradeId = GradeId,
            Grade = Grade.Name,
            Periodo = Periodo,
            Turno = Turno,
        };
    }
}
