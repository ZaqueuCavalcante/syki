using Syki.Shared;

namespace Syki.Back.Services;

public interface IMatriculasService
{
    Task<PeriodoDeMatriculaOut> CreatePeriodoDeMatricula(Guid faculdadeId, PeriodoDeMatriculaIn data);
    Task<List<PeriodoDeMatriculaOut>> GetPeriodosDeMatricula(Guid faculdadeId);
}
