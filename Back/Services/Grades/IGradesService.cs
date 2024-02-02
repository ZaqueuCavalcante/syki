using Syki.Shared;

namespace Syki.Back.Services;

public interface IGradesService
{
    Task<GradeOut> Create(Guid faculdadeId, GradeIn data);
    Task<List<DisciplinaOut>> GetDisciplinas(Guid faculdadeId, Guid id);
    Task<List<GradeOut>> GetAll(Guid faculdadeId);
}
