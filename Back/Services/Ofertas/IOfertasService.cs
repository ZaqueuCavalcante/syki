namespace Syki.Back.Services;

public interface IOfertasService
{
    Task<OfertaOut> Create(Guid faculdadeId, OfertaIn data);
    Task<List<OfertaOut>> GetAll(Guid faculdadeId);
}
