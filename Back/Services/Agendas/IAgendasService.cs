using Syki.Shared;

namespace Syki.Back.Services;

public interface IAgendasService
{
    Task<List<AgendaDiaOut>> GetAluno(Guid faculdadeId, Guid userId);
}
