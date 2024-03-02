namespace Syki.Back.Services;

public interface IMatriculasService
{
    Task Create(Guid faculdadeId, Guid userId, MatriculaTurmaIn data);
    Task<List<MatriculaTurmaOut>> GetTurmas(Guid faculdadeId, Guid userId);
}
