namespace Syki.Back.Services;

public interface IIndexService
{
    Task<IndexAdmOut> GetAllAdm();
    Task<IndexAlunoOut> GetAllAluno(Guid id);
}
