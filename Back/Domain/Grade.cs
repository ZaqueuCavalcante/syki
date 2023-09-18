using Syki.Dtos;

namespace Syki.Domain;

public class Grade
{
    public long Id { get; set; }
    
    public long FaculdadeId { get; set; }

    public long CursoId { get; set; }

    public string Nome { get; set; }

    public List<Disciplina> Disciplinas { get; set; }

    public GradeOut ToOut()
    {
        return new GradeOut
        {
            Id = Id,
            CursoId = CursoId,
            Nome = Nome,
            Disciplinas = Disciplinas.ConvertAll(d => d.ToOut()),
        };
    }
}
