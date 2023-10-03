using Syki.Dtos;

namespace Syki.Domain;

public class Disciplina
{
    public Guid Id { get; set; }

    public Guid FaculdadeId { get; set; }
    
    public string Nome { get; set; }

    public ushort CargaHoraria { get; set; }

    public List<CursoDisciplina> Vinculos { get; set; }

    public Disciplina(
        string nome,
        ushort cargaHoraria
    ) {
        Id = Guid.NewGuid();
        Nome = nome;
        CargaHoraria = cargaHoraria;
        Vinculos = new();
    }

    public Disciplina(
        string nome,
        Guid faculdadeId,
        ushort cargaHoraria
    ) {
        Id = Guid.NewGuid();
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
