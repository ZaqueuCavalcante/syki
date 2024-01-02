using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;

namespace Syki.Tests.Integration;

[TestFixture]
public class NotificationsIntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_a_notification_quando_o_usuario_nao_eh_academico(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, role);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" };

        // Act
        var response = await _client.PostAsync("/notifications", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
