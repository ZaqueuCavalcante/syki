namespace Syki.Back.Services;

public interface IIndexService
{
    Task<IndexAdmOut> GetAllAdm();
}
