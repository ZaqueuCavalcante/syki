using Syki.Back.Features.Academic.CreateCurso;
using Syki.Back.Features.Academic.CreateDisciplina;

namespace Syki.Back.Features.Academic.CreateGrade;

public class Grade
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid CursoId { get; set; }
    public Curso Curso { get; set; }
    public string Name { get; set; }
    public List<Disciplina> Disciplinas { get; set; }
    public List<GradeDisciplina> Vinculos { get; set; }

    public Grade(
        Guid institutionId,
        Guid cursoId,
        string name
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        CursoId = cursoId;
        Name = name;
        Disciplinas = [];
        Vinculos = [];
    }

    public GradeOut ToOut()
    {
        var result = new GradeOut
        {
            Id = Id,
            CursoId = CursoId,
            CursoNome = Curso.Name,
            Name = Name,
            Disciplinas = Disciplinas.ConvertAll(d => d.ToOut()),
        };

        foreach (var item in Vinculos)
        {
            var disciplina = result.Disciplinas.First(x => x.Id == item.DisciplinaId);
            disciplina.Periodo = item.Periodo;
            disciplina.Creditos = item.Creditos;
            disciplina.CargaHoraria = item.CargaHoraria;
        }

        return result;
    }
}
