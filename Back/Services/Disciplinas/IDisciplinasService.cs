namespace Syki.Back.Services;

public interface IDisciplinasService
{
    Task<DisciplinaOut> Create(Guid faculdadeId, DisciplinaIn data);
    Task<List<DisciplinaOut>> GetAll(Guid faculdadeId, Guid? cursoId);
}
