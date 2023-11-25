using Syki.Shared;

namespace Syki.Back.Services;

public interface ITurmasService
{
    Task<TurmaOut> Create(Guid faculdadeId, TurmaIn data);
    Task<List<TurmaOut>> GetAll(Guid faculdadeId);
}
