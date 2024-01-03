using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Database;
using Syki.Back.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class ExceptionsIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_lidar_com_um_internal_server_error()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        _ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        var academicoRole = await _ctx.Roles.FirstAsync(r => r.Name == "Academico");
        _ctx.Remove(academicoRole);
        await _ctx.SaveChangesAsync();

        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);

        // Act
        var response = await _client.PostAsync("/users", user.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        error.Message.Should().Be("Internal Server Error");
    }
}
