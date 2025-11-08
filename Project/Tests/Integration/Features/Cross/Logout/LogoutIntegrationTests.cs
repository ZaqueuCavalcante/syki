using System.Net.Http.Json;
using Exato.Shared.Features.Office.CriarEmpresa;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_logout()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response = await client.Cross.Logout();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = new CriarEmpresaIn { Nome = "Uber" };
        var createResponse = await client.Cross.PostAsJsonAsync("/api/office/empresas", body);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
