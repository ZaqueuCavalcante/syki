using Syki.Shared;

namespace Syki.Back.Services;

public interface IProfessoresService
{
    Task<ProfessorOut> Create(Guid faculdadeId, ProfessorIn data);
    Task<List<ProfessorOut>> GetAll(Guid faculdadeId);
}
