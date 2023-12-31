using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public class ExceptionsIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_lidar_com_um_internal_server_error()
    {
        // Arrange
        var client = _factory.CreateClient();
        using var scope = _factory.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        var academicoRole = await ctx.Roles.FirstAsync(r => r.Name == "Academico");
        ctx.Remove(academicoRole);
        await ctx.SaveChangesAsync();

        var faculdade = await client.CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);

        // Act
        var response = await client.PostAsync("/users", user.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        error.Message.Should().Be("Internal Server Error");
    }
}
