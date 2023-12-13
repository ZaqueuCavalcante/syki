using Syki.Shared;

namespace Syki.Back.Domain;

public class Disciplina
{
    public Guid Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public string Nome { get; set; }
    public ushort CargaHoraria { get; set; }
    public List<CursoDisciplina> Vinculos { get; set; }

    public Disciplina(
        Guid faculdadeId,
        string nome,
        ushort cargaHoraria
    ) {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        Nome = nome;
        CargaHoraria = cargaHoraria;
        Vinculos = [];
    }

    public DisciplinaOut ToOut()
    {
        return new DisciplinaOut
        {
            Id = Id,
            Nome = Nome,
            CargaHoraria = CargaHoraria,
            Cursos = Vinculos.ConvertAll(v => v.CursoId),
        };
    }
}
