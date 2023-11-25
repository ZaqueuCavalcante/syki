using Syki.Shared;

namespace Syki.Back.Services;

public interface IIndexService
{
    Task<IndexAcademicoOut> GetAllAcademico(Guid faculdadeId);
}
