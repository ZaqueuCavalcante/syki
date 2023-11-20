using Syki.Shared;

namespace Syki.Back.Services;

public interface IAlunosService
{
    Task<AlunoOut> Create(Guid faculdadeId, AlunoIn data);
    Task<List<DisciplinaOut>> GetDisciplinas(Guid userId);
    Task<List<AlunoOut>> GetAll(Guid faculdadeId);
}
