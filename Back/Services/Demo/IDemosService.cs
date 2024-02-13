using Syki.Shared;

namespace Syki.Back.Services;

public interface IDemosService
{
    Task<DemoOut> Create(DemoIn data);
    Task<DemoSetupOut> Setup(DemoSetupIn data);
}
