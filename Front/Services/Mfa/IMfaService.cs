using Syki.Shared.SetupMfa;
using Syki.Shared.GetMfaKey;

namespace Syki.Front.Services;

public interface IMfaService
{
    Task<GetMfaKeyOut> GetMfaKey();
    Task<bool> EnableUserMfa(string code);
}
