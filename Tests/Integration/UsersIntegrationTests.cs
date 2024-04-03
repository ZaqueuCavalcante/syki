using Syki.Back.Configs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test, Ignore("")]
    public async Task Deve_retornar_todos_os_usuarios()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");

        await client.RegisterUser(CreateUserIn.New(faculdade.Id, AuthorizationConfigs.Academico));
        await client.RegisterUser(CreateUserIn.New(faculdade.Id, AuthorizationConfigs.Professor));
        await client.RegisterUser(CreateUserIn.New(faculdade.Id, AuthorizationConfigs.Aluno));

        // Act
        var users = await client.GetAsync<List<CreateUserOut>>("/users");

        // Assert
        users.Count.Should().BeGreaterThanOrEqualTo(4);
    }
}
