namespace Syki.Back.Services;

public interface IIndexService
{
    Task<IndexAdmOut> GetAllAdm();
    Task<IndexAcademicoOut> GetAllAcademico(Guid faculdadeId);
    Task<IndexAlunoOut> GetAllAluno(Guid id);
}
