using Syki.Dtos;

namespace Syki.Domain;

public class Disciplina
{
    public long Id { get; set; }

    public long FaculdadeId { get; set; }
    
    public string Nome { get; set; }

    // TODO: se mudar, perco o historico?
    public short CargaHoraria { get; set; }

    public Disciplina() { }

    public Disciplina(
        string nome,
        long faculdadeId,
        short cargaHoraria
    ) {
        Nome = nome;
        FaculdadeId = faculdadeId;
        CargaHoraria = cargaHoraria;
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
