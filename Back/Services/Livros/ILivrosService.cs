using Syki.Shared;

namespace Syki.Back.Services;

public interface ILivrosService
{
    Task<LivroOut> Create(Guid faculdadeId, LivroIn data);
    Task<List<LivroOut>> GetAll(Guid faculdadeId);
}
