using Syki.Front.Services;
using Syki.Shared.SetupMfa;
using Syki.Shared.GetMfaKey;

namespace Syki.Tests.Mock;

public class DelayedMfaServiceMock : IMfaService
{
    public async Task<GetMfaKeyOut> GetMfaKey()
    {
        await Task.Delay(10_000);
        return new GetMfaKeyOut { Key = "LRZSGTSW5SEQFCWCNXHRNM5PZC7LFVBH" };
    }

    public async Task<SetupMfaOut> EnableUserMfa(string code)
    {
        await Task.Delay(0);

        if (code == "123456")
            return new SetupMfaOut { Ok = true };

        return new SetupMfaOut { Ok = false };
    }
}
