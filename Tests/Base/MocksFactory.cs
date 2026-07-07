using Microsoft.AspNetCore.Mvc.Testing;

namespace Syki.Tests.Base;

extern alias Mocks;

public class MocksFactory : WebApplicationFactory<Mocks::Program>
{
    public const string Url = "http://localhost:5678";

    public MocksFactory() : base()
    {
        UseKestrel(o => o.ListenLocalhost(5678));
    }
}
