using Syki.Shared;

namespace Syki.Back.Services;

public interface ICampiService
{
    Task<CampusOut> Create(Guid faculdadeId, CampusIn data);
    Task<CampusOut> Update(Guid faculdadeId, CampusOut data);
    Task<List<CampusOut>> GetAll(Guid faculdadeId);
}
