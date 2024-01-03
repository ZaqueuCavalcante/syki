using System.Net;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

[TestFixture]
public class HomeIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_redirecionar_pro_swagger()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.RequestMessage!.RequestUri!.ToString().Should().Contain("/swagger");
    }
}
