using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class NotificationsIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_a_notification()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, Academico);

        var body = new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" };

        // Act
        var response = await PostAsync<NotificationOut>("/notifications", body);

        // Assert
        response.Id.Should().NotBeEmpty();
        response.Title.Should().Be(body.Title);
        response.Description.Should().Be(body.Description);
    }
}
