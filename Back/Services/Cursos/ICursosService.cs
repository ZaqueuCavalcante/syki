namespace Syki.Back.Services;

public interface ICursosService
{
    Task<List<CursoOut>> GetAllWithDisciplinas(Guid faculdadeId);
    Task<List<CursoDisciplinaOut>> GetDisciplinas(Guid id, Guid faculdadeId);
}
