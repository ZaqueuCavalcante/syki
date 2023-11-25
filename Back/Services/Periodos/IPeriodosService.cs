using Syki.Shared;

namespace Syki.Back.Services;

public interface IPeriodosService
{
    Task<List<PeriodoOut>> GetAll(Guid faculdadeId);
}
