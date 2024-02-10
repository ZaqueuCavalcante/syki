using Syki.Shared;

namespace Syki.Back.Domain;

public class TurmaAluno
{
    public Guid TurmaId { get; set; }
    public Guid AlunoId { get; set; }
    public Situacao Situacao { get; set; }

    public TurmaAluno(
        Guid turmaId,
        Guid alunoId,
        Situacao situacao
    ) {
        TurmaId = turmaId;
        AlunoId = alunoId;
        Situacao = situacao;
    }
}
