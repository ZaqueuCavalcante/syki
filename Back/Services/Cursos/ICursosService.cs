using Syki.Shared;

namespace Syki.Back.Services;

public interface ICursosService
{
    Task<CursoOut> Create(Guid faculdadeId, CursoIn data);
    Task<List<CursoOut>> GetAll(Guid faculdadeId);
}
