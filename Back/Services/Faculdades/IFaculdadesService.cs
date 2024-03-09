namespace Syki.Back.Services;

public interface IFaculdadesService
{
    Task<FaculdadeOut> Create(FaculdadeIn data);
    Task<List<FaculdadeOut>> GetAll();
}
