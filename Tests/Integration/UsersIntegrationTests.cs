using Syki.Shared.CreateUser;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_retornar_todos_os_usuarios()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");

        await client.RegisterUser(CreateUserIn.New(faculdade.Id, Academico));
        await client.RegisterUser(CreateUserIn.New(faculdade.Id, Professor));
        await client.RegisterUser(CreateUserIn.New(faculdade.Id, Aluno));

        // Act
        var users = await client.GetAsync<List<CreateUserOut>>("/users");

        // Assert
        users.Count.Should().BeGreaterThanOrEqualTo(4);
    }
}
