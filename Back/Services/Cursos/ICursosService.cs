namespace Syki.Back.Services;

public interface ICursosService
{
    Task<List<CursoDisciplinaOut>> GetDisciplinas(Guid id, Guid faculdadeId);
}
