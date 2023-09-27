using Syki.Dtos;

namespace Syki.Domain;

public class Disciplina
{
    public long Id { get; set; }

    public long FaculdadeId { get; set; }
    
    public string Nome { get; set; }

    public ushort CargaHoraria { get; set; }

    public List<CursoDisciplina> Vinculos { get; set; }

    public Disciplina() { }

    public Disciplina(
        string nome,
        long faculdadeId,
        ushort cargaHoraria
    ) {
        Nome = nome;
        FaculdadeId = faculdadeId;
        CargaHoraria = cargaHoraria;
        Vinculos = new();
    }

    public DisciplinaOut ToOut()
    {
        return new DisciplinaOut
        {
            Id = Id,
            Nome = Nome,
            CargaHoraria = CargaHoraria,
        };
    }
}
