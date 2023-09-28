using Syki.Dtos;

namespace Syki.Domain;

public class Grade
{
    public long Id { get; set; }
    
    public long FaculdadeId { get; set; }

    public long CursoId { get; set; }

    public string Nome { get; set; }

    public List<Disciplina> Disciplinas { get; set; }

    public List<GradeDisciplina> Vinculos { get; set; }

    public GradeOut ToOut()
    {
        var result = new GradeOut
        {
            Id = Id,
            CursoId = CursoId,
            Nome = Nome,
            Disciplinas = Disciplinas.ConvertAll(d => d.ToOut()),
        };

        if (Vinculos != null)
        {
            foreach (var item in Vinculos)
            {
                var disciplina = result.Disciplinas.First(x => x.Id == item.DisciplinaId);
                disciplina.Periodo = item.Periodo;
                disciplina.Creditos = item.Creditos;
                disciplina.CargaHoraria = item.CargaHoraria;
            }
        }

        return result;
    }
}
