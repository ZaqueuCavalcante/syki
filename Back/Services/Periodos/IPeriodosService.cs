using Syki.Shared;

namespace Syki.Back.Services;

public interface IPeriodosService
{
    Task<PeriodoOut> Create(Guid faculdadeId, PeriodoIn data);
    Task<List<PeriodoOut>> GetAll(Guid faculdadeId);
}
