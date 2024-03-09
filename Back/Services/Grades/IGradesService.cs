namespace Syki.Back.Services;

public interface IGradesService
{
    Task<List<DisciplinaOut>> GetDisciplinas(Guid faculdadeId, Guid id);
    Task<List<GradeOut>> GetAll(Guid faculdadeId);
}
