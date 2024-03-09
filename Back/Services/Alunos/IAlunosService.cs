namespace Syki.Back.Services;

public interface IAlunosService
{
    Task<List<DisciplinaOut>> GetDisciplinas(Guid userId);
    Task<List<AlunoOut>> GetAll(Guid faculdadeId);
}
