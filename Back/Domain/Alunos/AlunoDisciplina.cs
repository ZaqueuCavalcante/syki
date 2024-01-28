using Syki.Shared;

namespace Syki.Back.Domain;

public class AlunoDisciplina
{
    public Guid Id { get; set; }
    public Guid AlunoId { get; set; }
    public Guid DisciplinaId { get; set; }
    public Situacao Situacao { get; set; }

    public AlunoDisciplina(
        Guid alunoId,
        Guid disciplinaId,
        Situacao situacao
    ) {
        Id = Guid.NewGuid();
        AlunoId = alunoId;
        DisciplinaId = disciplinaId;
        Situacao = situacao;
    }
}
