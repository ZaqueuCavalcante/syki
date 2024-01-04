using Syki.Shared;
using Syki.Front.Services;

namespace Syki.Tests.Mock;

public class MfaServiceMock : IMfaService
{
    public async Task<MfaKeyOut> GetMfaKey()
    {
        await Task.Delay(0);
        return new MfaKeyOut { Key = "LRZSGTSW5SEQFCWCNXHRNM5PZC7LFVBH" };
    }

    public async Task<MfaSetupOut> EnableUserMfa(string code)
    {
        await Task.Delay(0);

        if (code == "123456")
            return new MfaSetupOut { Ok = true };

        return new MfaSetupOut { Ok = false };
    }
}
