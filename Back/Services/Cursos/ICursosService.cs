namespace Syki.Back.Services;

public interface ICursosService
{
    Task<CursoOut> Create(Guid faculdadeId, CursoIn data);
    Task<List<CursoOut>> GetAll(Guid faculdadeId);
    Task<List<CursoOut>> GetAllWithDisciplinas(Guid faculdadeId);
    Task<List<CursoDisciplinaOut>> GetDisciplinas(Guid id, Guid faculdadeId);
}
