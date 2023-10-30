using Syki.Shared;

namespace Syki.Front.Services;

public interface IMfaService
{
    Task<MfaKeyOut> GetMfaKey();
    Task<MfaSetupOut> EnableUserMfa(string code);
}
