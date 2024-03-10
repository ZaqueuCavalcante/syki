using System.Net;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // [Test]
    public async Task Deve_redirecionar_pro_swagger()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.RequestMessage!.RequestUri!.ToString().Should().Contain("/swagger");
    }
}
