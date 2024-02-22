using Syki.Shared;

namespace Syki.Back.Services;

public interface IDemosService
{
    Task<DemoSetupOut> Setup(DemoSetupIn data);
}
