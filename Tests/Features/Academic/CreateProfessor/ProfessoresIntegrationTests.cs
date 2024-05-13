namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_professor()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var professor = await client.CreateProfessor("Chico");

        // Assert
        professor.Id.Should().NotBeEmpty();
        professor.Name.Should().Be("Chico");
    }

    [Test, Ignore("")]
    public async Task Deve_criar_varios_professores_para_uma_mesma_institution()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        await client.CreateProfessor("Chico");
        await client.CreateProfessor("Ana");

        // Assert
        var professores = await client.GetAsync<List<TeacherOut>>("/professores");
        professores.Should().HaveCount(2);
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_apenas_os_professores_da_institution_do_usuario_logado()
    {
        // Arrange
        var clientNovaRoma = await _factory.LoggedAsAcademic();
        var clientUfpe = await _factory.LoggedAsAcademic();

        await clientNovaRoma.CreateProfessor("Chico");
        await clientUfpe.CreateProfessor("Ana");

        // Act
        var professores = await clientNovaRoma.GetAsync<List<TeacherOut>>("/professores");

        // Assert
        professores.Should().HaveCount(1);
        professores[0].Name.Should().Be("Chico");
    }
}
