using Syki.Front.Services;
using Syki.Shared.SetupMfa;
using Syki.Shared.GetMfaKey;
using RichardSzalay.MockHttp;
using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Services;

public class MfaServiceTests : BunitTestContext
{
    [Test]
    public async Task Should_get_mfa_key()
    {
        // Arrange
        Services.AddScoped<IMfaService, MfaService>();

        var response = new GetMfaKeyOut { Key = "LRZSGTSW5SEQFCWCNXHRNM5PZC7LFVBH" };
        var mock = Services.AddMockHttpClient();
        mock.When(HttpMethod.Get, "/mfa/key").RespondJson(response);

        var mfaService = Services.GetService<IMfaService>()!;

        // Act
        var result = await mfaService.GetMfaKey();

        // Assert
        result.Key.Should().Be(response.Key);
    }

    [Test]
    public async Task Should_enable_user_mfa()
    {
        // Arrange
        Services.AddScoped<IMfaService, MfaService>();

        var response = new SetupMfaOut { Ok = true };
        var mock = Services.AddMockHttpClient();
        mock.When(HttpMethod.Post, "/mfa/setup").RespondJson(response);

        var mfaService = Services.GetService<IMfaService>()!;

        // Act
        var result = await mfaService.EnableUserMfa("951753");

        // Assert
        result.Should().BeTrue();
    }
}
