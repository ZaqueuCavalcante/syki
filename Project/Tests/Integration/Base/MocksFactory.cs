using Microsoft.AspNetCore.Mvc.Testing;

namespace Exato.Tests.Integration.Base;

extern alias Mocks;

public class MocksFactory : WebApplicationFactory<Mocks::Program>
{
    public MocksFactory() : base()
    {
        UseKestrel(o => o.ListenLocalhost(5678));
    }
}
