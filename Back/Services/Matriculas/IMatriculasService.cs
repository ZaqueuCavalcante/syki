using Syki.Shared;

namespace Syki.Back.Services;

public interface IMatriculasService
{
    Task<PeriodoDeMatriculaOut> CreatePeriodoDeMatricula(Guid faculdadeId, PeriodoDeMatriculaIn data);
    Task<List<PeriodoDeMatriculaOut>> GetPeriodosDeMatricula(Guid faculdadeId);
    Task<PeriodoDeMatriculaOut> GetPeriodoDeMatriculaAtual(Guid faculdadeId);
    Task Create(Guid faculdadeId, Guid userId, MatriculaTurmaIn data);
    Task<List<MatriculaTurmaOut>> GetTurmas(Guid faculdadeId, Guid userId);
}
